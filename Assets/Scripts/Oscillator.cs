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

  [Range(10, 22000)]
  public float maxFrequency = 5500;

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
  }

  void Start() {
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
    double[] frequencyModValues = frequencyInput.IsConnected() ?
      frequencyInput.GetValues(sample, count, modules) :
      Enumerable.Repeat(0d, count).ToArray();
    double[] volumeModValues = volumeInput.IsConnected() ?
      volumeInput.GetValues(sample, count, modules) :
      Enumerable.Repeat(0d, count).ToArray();
    double[] waveformModValues = waveformInput.IsConnected() ?
      waveformInput.GetValues(sample, count, modules) :
      Enumerable.Repeat(0d, count).ToArray();

    defaultVoice.frequency = frequencyKnob.value * maxFrequency;
    float volume = volumeKnob.value;
    float waveform = Mathf.Lerp(-1, 1, waveformKnob.value);

    bool midi = midiInput.IsConnected();

    double[] values = new double[count];

    for (int i = 0; i < count; i++) {
      double thisSample = sample + i;
      lastSample = thisSample;

      // Modulate frequency between half and twice its current value.
      float frequencyMultiplier = Mathf.Pow(2, (float)frequencyModValues[i]);
      // Modulate volume between zero and twice its current value.
      float currentVolume = volume * (1 + (float)volumeModValues[i]);
      // Add waveform mod value to knob value.
      float currentWaveform = (float)(waveform + waveformModValues[i]);
      float waveformPhase = Mathf.InverseLerp(-1, 1, currentWaveform);

      Voice[] voices = midi ? midiVoices.Values.ToArray() : new[] { defaultVoice };

      values[i] = 0;

      foreach (Voice voice in voices) {
        double currentFrequency = voice.frequency * frequencyMultiplier;

        double sampleIncrement = thisSample - voice.phaseSample;
        float phaseIncrement = (float)(sampleIncrement * currentFrequency / sampleFrequency);
        voice.phaseSample = thisSample;
        voice.phase = (voice.phase + phaseIncrement) % 1;

        float currentVoiceVolume = (float)voice.volume * currentVolume;
        if (midi && envelope != null) {
          float? envVolume = envelope.GetVolume(
            voice.pressSample, voice.releaseSample, thisSample, sampleFrequency);

          if (envVolume == null) {
            midiVoices.Remove(voice.note);
            currentVoiceVolume = 0;
          }
          else {
            currentVoiceVolume *= (float)envVolume;
          }
        }

        double value = 0;

        if (wave == WaveType.Sine) {
          value = Mathf.Sin((voice.phase * TAU));

          for (int w = 1; w <= Mathf.Abs(currentWaveform) * waveformSineMultiplier; w++) {
            float m = 1 + w * 2;
            value += Mathf.Sin((voice.phase * m * TAU)) / m * Mathf.Sign(currentWaveform);
          }
        }
        else if (wave == WaveType.Pulse) {
          value = Mathf.Sign(voice.phase - waveformPhase);
        }
        else if (wave == WaveType.Sloped) {
          value = voice.phase < waveformPhase ?
            Mathf.Lerp(-1, 1, Mathf.InverseLerp(0, waveformPhase, voice.phase)) :
            Mathf.Lerp(1, -1, Mathf.InverseLerp(waveformPhase, 1, voice.phase));
        }
        else if (wave == WaveType.Noise) {
          value = rand.NextDouble() * 2 - 1;
        }

        values[i] += value * currentVoiceVolume;
      }
    }

    return values;
  }
}
