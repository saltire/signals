using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum WaveType {
  Sine,
  Pulse,
  Sloped,
  Noise,
};

class Voice {
  public int note;
  public double frequency;
  public float volume;
  public double pressSample;
  public double? releaseSample;
  public double phaseSample;
  public float phase;
}

public class Oscillator : SignalModule {
  public SignalInput frequencyInput;
  public SignalInput volumeInput;
  public SignalInput waveformInput;
  public RangeControl frequencyKnob;
  public RangeControl volumeKnob;
  public RangeControl waveformKnob;
  public Button sineButton;
  public Button pulseButton;
  public Button slopedButton;
  public Button noiseButton;
  Button[] buttons;
  MIDIInput midiInput;
  Envelope envelope;

  public WaveType wave = WaveType.Sine;

  Voice defaultVoice = new Voice { volume = 1 };
  Dictionary<int, Voice> midiVoices = new Dictionary<int, Voice>();

  double lastSample = 0;
  double sampleFrequency = 48000;
  const float TAU = Mathf.PI * 2;

  int waveformSineMultiplier = 20;

  System.Random rand;

  void Awake() {
    midiInput = GetComponentInChildren<MIDIInput>();
    envelope = GetComponentInChildren<Envelope>();

    rand = new System.Random();

    if (wave == WaveType.Sine) {
      sineButton.SetGlow(true);
    }
    else if (wave == WaveType.Pulse) {
      pulseButton.SetGlow(true);
    }
    else if (wave == WaveType.Sloped) {
      slopedButton.SetGlow(true);
    }
    else if (wave == WaveType.Noise) {
      noiseButton.SetGlow(true);
    }
  }

  public override void OnButtonClick(Button button) {
    sineButton.SetGlow(false);
    pulseButton.SetGlow(false);
    slopedButton.SetGlow(false);
    noiseButton.SetGlow(false);

    button.SetGlow(true);

    if (button == sineButton) {
      wave = WaveType.Sine;
    }
    else if (button == pulseButton) {
      wave = WaveType.Pulse;
    }
    else if (button == slopedButton) {
      wave = WaveType.Sloped;
    }
    else if (button == noiseButton) {
      wave = WaveType.Noise;
    }
  }

  public override void OnMIDIEvent(int note, float velocity) {
    if (velocity > 0) {
      midiVoices[note] = new Voice {
        frequency = MIDI.notes[note],
        volume = velocity,
        pressSample = lastSample,
        releaseSample = null,
      };
    }
    else {
      midiVoices[note].releaseSample = lastSample;
    }
  }

  public override double[] GetValues(double sample, int count, Stack<SignalModule> modules,
    SignalOutput output) {
    double[] frequencyAdjustValues = frequencyInput.IsConnected() ?
      frequencyInput.GetValues(sample, count, modules) :
      Enumerable.Repeat(0d, count).ToArray();
    double[] volumeAdjustValues = volumeInput.IsConnected() ?
      volumeInput.GetValues(sample, count, modules) :
      Enumerable.Repeat(0d, count).ToArray();
    double[] waveformAdjustValues = waveformInput.IsConnected() ?
      waveformInput.GetValues(sample, count, modules) :
      Enumerable.Repeat(0d, count).ToArray();

    defaultVoice.frequency = frequencyKnob.value;
    float volume = volumeKnob.value;
    float waveform = waveformKnob.value;

    bool midi = midiInput.IsConnected();

    double[] values = new double[count];

    for (int i = 0; i < count; i++) {
      double thisSample = sample + i;
      lastSample = thisSample;

      float currentWaveform = (float)(waveform + waveformAdjustValues[i]);

      Voice[] voices = midi ? midiVoices.Values.ToArray() : new[] { defaultVoice };

      values[i] = 0;

      foreach (Voice voice in voices) {
        double currentFrequency = voice.frequency + frequencyAdjustValues[i];

        double sampleIncrement = thisSample - voice.phaseSample;
        float phaseIncrement = (float)(sampleIncrement * currentFrequency / sampleFrequency);
        voice.phaseSample = thisSample;
        voice.phase = (voice.phase + phaseIncrement) % 1;

        float currentVolume = (float)(voice.volume * volume * (1 + volumeAdjustValues[i]));
        if (midi && envelope != null) {
          float? envVolume = envelope.GetVolume(
            voice.pressSample, voice.releaseSample, thisSample, sampleFrequency);

          if (envVolume == null) {
            midiVoices.Remove(voice.note);
            currentVolume = 0;
          }
          else {
            currentVolume *= (float)envVolume;
          }
        }

        double value = 0;

        if (wave == WaveType.Sine) {
          value = Mathf.Sin((voice.phase * TAU));

          float sineWaveform = Mathf.Lerp(-1, 1, currentWaveform);
          for (int w = 1; w <= Mathf.Abs(sineWaveform) * waveformSineMultiplier; w++) {
            float m = 1 + w * 2;
            value += Mathf.Sin((voice.phase * m * TAU)) / m * Mathf.Sign(sineWaveform);
          }
        }
        else if (wave == WaveType.Pulse) {
          value = Mathf.Sign(voice.phase - currentWaveform);
        }
        else if (wave == WaveType.Sloped) {
          value = voice.phase < currentWaveform ?
            Mathf.Lerp(-1, 1, Mathf.InverseLerp(0, currentWaveform, voice.phase)) :
            Mathf.Lerp(1, -1, Mathf.InverseLerp(currentWaveform, 1, voice.phase));
        }
        else if (wave == WaveType.Noise) {
          value = rand.NextDouble() * 2 - 1;
        }

        values[i] += value * currentVolume;
      }
    }

    return values;
  }
}
