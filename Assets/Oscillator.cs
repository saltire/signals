using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WaveType {
  Sine,
  Square,
  Sawtooth,
  Noise,
};

public class Oscillator : MonoBehaviour, ISignalNode {
  const double TAU = Mathf.PI * 2.0;

  double phase;
  double lastSample = 0;
  double sampleFrequency = 48000;

  public double frequency = 440;
  public float frequencyAdjustSensitivity = 1;

  public float volume = 0.1f;
  public float volumeAdjustSensitivity = .1f;

  public WaveType wave = WaveType.Sine;

  public SignalInput frequencyAdjustInput;
  public SignalInput volumeAdjustInput;

  System.Random rand;

  void Awake() {
    rand = new System.Random();
  }

  public void AdjustFrequency(float speed) {
    // TODO: rounding options; exponentially increase speed; accelerate speed over time
    frequency = Mathf.Max(0, (float)frequency + speed * Time.deltaTime * frequencyAdjustSensitivity);
  }

  public void AdjustVolume(float speed) {
    volume = Mathf.Max(0, volume + speed * Time.deltaTime * volumeAdjustSensitivity);
  }

  public float GetValue(double sample, Stack<ISignalNode> nodes) {
    double sampleIncrement = sample - lastSample;
    lastSample = sample;

    double currentFrequency = frequency;
    if (frequencyAdjustInput.IsConnected()) {
      currentFrequency += frequencyAdjustInput.GetValue(sample, nodes);
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

    if (volumeAdjustInput.IsConnected()) {
      value *= 1 + volumeAdjustInput.GetValue(sample, nodes);
    }

    return value * volume;
  }
}
