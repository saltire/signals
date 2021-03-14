using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NAudio.Dsp;

public enum FilterType {
  LowPass,
  HighPass,
}

public class Filter : SignalModule {
  public SignalInput input;
  public SignalInput frequencyModInput;
  public SignalInput intensityInput; // Ranges from 0 to 1, e.g. from an envelope generator.
  public Button lowPassButton;
  public Button highPassButton;
  public RangeControl cutoffKnob;
  public RangeControl qKnob;

  public FilterType type;
  float cutoff;
  float q;

  FilterType lastType;
  float lastCutoff;
  float lastQ;

  float sampleRate = 48000;

  BiQuadFilter filter;

  void Awake() {
    input = GetComponentInChildren<SignalInput>();

    ReadKnobs();

    lastType = type;
    lastCutoff = cutoff;
    lastQ = q;

    if (type == FilterType.LowPass) {
      filter = BiQuadFilter.LowPassFilter(sampleRate, cutoff, q);
      lowPassButton.SetGlow(true);
    }
    else if (type == FilterType.HighPass) {
      filter = BiQuadFilter.HighPassFilter(sampleRate, cutoff, q);
      highPassButton.SetGlow(true);
    }
  }

  void Update() {
    ReadKnobs();

    if (type != lastType || cutoff != lastCutoff || q != lastQ) {
      UpdateFilter(type, cutoff, q);
    }
  }

  void ReadKnobs() {
    cutoff = Mathf.Lerp(10, 11000, cutoffKnob.value);
    q = Mathf.Lerp(0.01f, 50, qKnob.value);
  }

  void UpdateFilter(FilterType newType, float newCutoff, float newQ) {
    lastType = newType;

    if (type == FilterType.LowPass) {
      lowPassButton.SetGlow(true);
      highPassButton.SetGlow(false);
    }
    else if (type == FilterType.HighPass) {
      highPassButton.SetGlow(true);
      lowPassButton.SetGlow(false);
    }

    UpdateFilter(newCutoff, newQ);
  }

  void UpdateFilter(float newCutoff, float newQ) {
    lastCutoff = newCutoff;
    lastQ = newQ;

    if (type == FilterType.LowPass) {
      filter.SetLowPassFilter(sampleRate, newCutoff, newQ);
    }
    else if (type == FilterType.HighPass) {
      filter.SetHighPassFilter(sampleRate, newCutoff, newQ);
    }
  }

  public override void OnButtonClick(Button button) {
    if (button == lowPassButton) {
      type = FilterType.LowPass;
    }
    else if (button == highPassButton) {
      type = FilterType.HighPass;
    }
  }

  public override double[] GetValues(double sample, int count, Stack<SignalModule> modules,
    SignalOutput output) {
    bool frequencyConnected = frequencyModInput.IsConnected();
    bool intensityConnected = intensityInput.IsConnected();

    double[] frequencyModValues = frequencyConnected ?
      frequencyModInput.GetValues(sample, count, modules) :
      Enumerable.Repeat(0d, count).ToArray();
    double[] intensityValues = intensityConnected ?
      intensityInput.GetValues(sample, count, modules) :
      Enumerable.Repeat(1d, count).ToArray(); // Default to 1; full intensity.

    double[] values = new double[count];
    double[] inputValues = input.GetValues(sample, count, modules);

    for (int i = 0; i < count; i++) {
      if (frequencyConnected) {
        // Modulate frequency between half and twice its current value.
        float frequencyMultiplier = Mathf.Pow(2, (float)frequencyModValues[i]);

        UpdateFilter(cutoff * frequencyMultiplier, q);
      }

      values[i] = (double)filter.Transform((float)inputValues[i]);

      if (intensityValues[i] < 1) {
        values[i] = Mathf.Lerp((float)inputValues[i], (float)values[i], (float)intensityValues[i]);
      }
    }

    return values;
  }
}
