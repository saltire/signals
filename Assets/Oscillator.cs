﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum WaveType {
  Sine,
  Square,
  Sawtooth,
  Noise,
};

public class Oscillator : MonoBehaviour, IPointerClickHandler {
  const double TAU = Mathf.PI * 2.0;

  double phase;
  double sampleCount = 0;
  double sampleFrequency = 48000;

  public double frequency = 440;
  public float frequencyAdjustSensitivity = 1;

  public float volume = 0.1f;
  public float volumeAdjustSensitivity = .1f;

  public WaveType wave = WaveType.Sine;

  public Oscillator frequencyLFO;
  public float frequencyLFOAmount = 0;

  public Oscillator volumeLFO;
  public float volumeLFOAmount = 0;

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
    return GetValue(sampleCount + 1);
  }

  public float GetValue(double newSampleCount) {
    double sampleIncrement = newSampleCount - sampleCount;
    sampleCount = newSampleCount;

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

  public void OnPointerClick(PointerEventData data) {
    Util.Log("Pointer click", gameObject.name, data);
  }
}
