using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NAudio.Dsp;

public enum FilterType {
  LowPass,
  HighPass,
}

public class Filter : SignalNode {
  SignalInput input;
  RangeControl cutoffKnob;
  public Button lowPassButton;
  public Button highPassButton;

  public FilterType type;

  float lastCutoff;
  FilterType lastType;

  float q = 1;

  float sampleRate = 48000;

  BiQuadFilter filter;

  void Awake() {
    input = GetComponentInChildren<SignalInput>();
    cutoffKnob = GetComponentInChildren<Knob>();

    float cutoff = cutoffKnob.value;

    lastType = type;
    lastCutoff = cutoff;

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

    if (type != lastType || cutoff != lastCutoff) {
      lastType = type;
      lastCutoff = cutoff;

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

  public override double[] GetValues(double sample, int count, Stack<SignalNode> nodes) {
    return input.GetValues(sample, count, nodes)
      .Select(s => (double)filter.Transform((float)s)).ToArray();
  }
}
