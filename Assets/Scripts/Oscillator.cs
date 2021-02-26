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

  public WaveType wave = WaveType.Sine;

  double phase;
  double lastSample = 0;
  double sampleFrequency = 48000;
  const double TAU = Mathf.PI * 2.0;

  System.Random rand;

  void Awake() {
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

  public override double[] GetValues(double sample, int count, Stack<SignalNode> nodes) {
    double[] values = new double[count];

    double[] frequencyAdjustValues = frequencyInput.IsConnected() ?
      frequencyInput.GetValues(sample, count, nodes) :
      Enumerable.Repeat(0d, count).ToArray();
    double[] volumeAdjustValues = volumeInput.IsConnected() ?
      volumeInput.GetValues(sample, count, nodes) :
      Enumerable.Repeat(0d, count).ToArray();
    double[] waveformAdjustValues = waveformInput.IsConnected() ?
      waveformInput.GetValues(sample, count, nodes) :
      Enumerable.Repeat(0d, count).ToArray();

    float frequency = frequencyKnob.value;
    float volume = volumeKnob.value;
    float waveform = waveformKnob.value;

    for (int i = 0; i < count; i++) {
      double currentFrequency = frequency + frequencyAdjustValues[i];
      float currentWaveform = (float)(waveform + waveformAdjustValues[i]);
      float currentVolume = (float)(volume * (1 + volumeAdjustValues[i]));

      double thisSample = sample + i;
      double sampleIncrement = thisSample - lastSample;
      lastSample = thisSample;
      double phaseIncrement = sampleIncrement * currentFrequency / sampleFrequency;
      phase = (phase + phaseIncrement) % 1;

      double value = 0;

      if (wave == WaveType.Sine) {
        value = Mathf.Sin((float)(phase * TAU));
      }
      else if (wave == WaveType.Pulse) {
        value = phase >= currentWaveform ? -1 : 1;
      }
      else if (wave == WaveType.Sloped) {
        value = phase < currentWaveform ?
          Mathf.Lerp(-1, 1, Mathf.InverseLerp(0, currentWaveform, (float)phase)) :
          Mathf.Lerp(1, -1, Mathf.InverseLerp(currentWaveform, 1, (float)phase));
      }
      else if (wave == WaveType.Noise) {
        value = rand.NextDouble() * 2 - 1;
      }

      values[i] = value * currentVolume;
    }

    return values;
  }
}
