using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WaveType {
  Sine,
  Square,
  Sawtooth,
  Noise,
};

public class Oscillator : MonoBehaviour {
  const double TAU = Mathf.PI * 2.0;

  public double frequency = 440;
  double phase;
  double samples = 0;
  double sampleFrequency = 48000;

  public WaveType wave = WaveType.Sine;

  public float volume = 0.1f;

  public Oscillator frequencyLFO;
  public float frequencyLFOAmount = 0;

  public Oscillator volumeLFO;
  public float volumeLFOAmount = 0;

  System.Random rand;

  void Awake() {
    rand = new System.Random();
  }

  void OnAudioFilterRead(float[] data, int channels) {
    for (int i = 0; i < data.Length; i += channels) {
      float value = GetValue();

      data[i] += value * volume;

      if (channels == 2) {
        data[i + 1] = data[i];
      }
    }
  }

  public float GetValue() {
    return GetValue(samples + 1);
  }

  public float GetValue(double newSamples) {
    double sampleIncrement = newSamples - samples;
    samples = newSamples;

    double currentFrequency = frequency;
    if (frequencyLFO != null && frequencyLFOAmount != 0) {
      currentFrequency = frequency + frequencyLFO.GetValue() * frequencyLFOAmount;
    }
    double increment = sampleIncrement * currentFrequency / sampleFrequency;

    phase = (phase + increment) % 1;

    float value = 0;

    if (wave == WaveType.Sine) {
      value = Mathf.Sin((float)(phase * TAU));
    }
    else if (wave == WaveType.Square) {
      value = Mathf.Sign(Mathf.Sin((float)(phase * TAU)));
    }
    else if (wave == WaveType.Sawtooth) {
      value = (float)(phase * 2 - 1);
    }
    else if (wave == WaveType.Noise) {
      value = (float)rand.NextDouble() * 2 - 1;
    }

    if (volumeLFO != null && volumeLFOAmount != 0) {
      value *= 1 + volumeLFO.GetValue() * volumeLFOAmount;
    }

    return value * volume;
  }
}
