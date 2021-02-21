using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NAudio.Dsp;

public enum FilterType {
  LowPass,
  HighPass,
}

public class Filter : MonoBehaviour, ISignalNode {
  SignalInput input;
  Knob cutoffKnob;

  float lastCutoff;

  float q = 1;

  public FilterType type;
  FilterType lastType;

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
    }
    else if (type == FilterType.HighPass) {
      filter = BiQuadFilter.HighPassFilter(sampleRate, cutoff, q);
    }
  }

  void Update() {
    float cutoff = cutoffKnob.value;

    if (type != lastType || cutoff != lastCutoff) {
      lastType = type;
      lastCutoff = cutoff;

      if (type == FilterType.LowPass) {
        filter.SetLowPassFilter(sampleRate, cutoff, q);
      }
      else if (type == FilterType.HighPass) {
        filter.SetHighPassFilter(sampleRate, cutoff, q);
      }
    }
  }

  public double GetValue(double sample, Stack<ISignalNode> nodes) {
    return filter.Transform((float)input.GetValue(sample, nodes));
  }

  public double[] GetValues(double sample, int count, Stack<ISignalNode> nodes) {
    return input.GetValues(sample, count, nodes)
      .Select(s => (double)filter.Transform((float)s)).ToArray();
  }
}
