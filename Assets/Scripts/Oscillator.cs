﻿using System.Collections;
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
  public double phase;
}

public class Oscillator : SignalNode {
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

  Voice defaultVoice = new Voice {};
  Dictionary<int, Voice> midiVoices = new Dictionary<int, Voice>();

  double lastSample = 0;
  double sampleFrequency = 48000;
  const double TAU = Mathf.PI * 2.0;

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

  public override void OnMIDIEvent(int note, float volume) {
    if (volume > 0) {
      midiVoices[note] = new Voice {
        frequency = MIDI.notes[note],
        volume = volume,
        pressSample = lastSample,
        releaseSample = null,
      };
    }
    else {
      midiVoices[note].releaseSample = lastSample;
    }
  }

  public override double[] GetValues(double sample, int count, Stack<SignalNode> nodes) {
    double[] frequencyAdjustValues = frequencyInput.IsConnected() ?
      frequencyInput.GetValues(sample, count, nodes) :
      Enumerable.Repeat(0d, count).ToArray();
    double[] volumeAdjustValues = volumeInput.IsConnected() ?
      volumeInput.GetValues(sample, count, nodes) :
      Enumerable.Repeat(0d, count).ToArray();
    double[] waveformAdjustValues = waveformInput.IsConnected() ?
      waveformInput.GetValues(sample, count, nodes) :
      Enumerable.Repeat(0d, count).ToArray();

    defaultVoice.frequency = frequencyKnob.value;
    defaultVoice.volume = volumeKnob.value;
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
        double phaseIncrement = sampleIncrement * currentFrequency / sampleFrequency;
        voice.phaseSample = thisSample;
        voice.phase = (voice.phase + phaseIncrement) % 1;

        float currentVolume = (float)(voice.volume * (1 + volumeAdjustValues[i]));
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
          value = Mathf.Sin((float)(voice.phase * TAU));
        }
        else if (wave == WaveType.Pulse) {
          value = voice.phase >= currentWaveform ? -1 : 1;
        }
        else if (wave == WaveType.Sloped) {
          value = voice.phase < currentWaveform ?
            Mathf.Lerp(-1, 1, Mathf.InverseLerp(0, currentWaveform, (float)voice.phase)) :
            Mathf.Lerp(1, -1, Mathf.InverseLerp(currentWaveform, 1, (float)voice.phase));
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
