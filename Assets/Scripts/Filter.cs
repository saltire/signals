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
  SignalInput input;
  public Button lowPassButton;
  public Button highPassButton;
  public RangeControl cutoffKnob;
  public RangeControl qKnob;

  public FilterType type;

  FilterType lastType;
  float lastCutoff;
  float lastQ;

  float sampleRate = 48000;

  BiQuadFilter filter;

  void Awake() {
    input = GetComponentInChildren<SignalInput>();

    float cutoff = cutoffKnob.value;
    float q = qKnob.value;

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
    float cutoff = cutoffKnob.value;
    float q = qKnob.value;

    if (type != lastType || cutoff != lastCutoff || q != lastQ) {
      lastType = type;
      lastCutoff = cutoff;
      lastQ = q;

      if (type == FilterType.LowPass) {
        filter.SetLowPassFilter(sampleRate, cutoff, q);
        lowPassButton.SetGlow(true);
        highPassButton.SetGlow(false);
      }
      else if (type == FilterType.HighPass) {
        filter.SetHighPassFilter(sampleRate, cutoff, q);
        highPassButton.SetGlow(true);
        lowPassButton.SetGlow(false);
      }
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
    return input.GetValues(sample, count, modules)
      .Select(s => (double)filter.Transform((float)s)).ToArray();
  }
}
