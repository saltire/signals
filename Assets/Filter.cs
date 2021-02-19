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
  [Range(1, 22000)]
  public float cutoff = 400;
  float lastCutoff;

  float q = 1;

  public FilterType type;
  FilterType lastType;

  float sampleRate = 48000;

  BiQuadFilter filter;

  SignalInput input;

  void Start() {
    input = GetComponentInChildren<SignalInput>();

    lastType = type;
    lastCutoff = cutoff = Mathf.Max(1, cutoff);

    if (type == FilterType.LowPass) {
      filter = BiQuadFilter.LowPassFilter(sampleRate, cutoff, q);
    }
    else if (type == FilterType.HighPass) {
      filter = BiQuadFilter.HighPassFilter(sampleRate, cutoff, q);
    }
  }

  void Update() {
    if (type != lastType || cutoff != lastCutoff) {
      lastType = type;
      lastCutoff = cutoff = Mathf.Max(1, cutoff);

      if (type == FilterType.LowPass) {
        filter.SetLowPassFilter(sampleRate, cutoff, q);
      }
      else if (type == FilterType.LowPass) {
        filter.SetHighPassFilter(sampleRate, cutoff, q);
      }
    }
  }

  public double GetValue(double sample, Stack<ISignalNode> nodes) {
    // return filter.ProcessSample(input.GetValue(sample, nodes));
    return filter.Transform((float)input.GetValue(sample, nodes));
  }

  public double[] GetValues(double sample, int count, Stack<ISignalNode> nodes) {
    // return filter.ProcessSamples(input.GetValues(sample, count, nodes));
    return input.GetValues(sample, count, nodes)
      .Select(s => (double)filter.Transform((float)s)).ToArray();
  }
}
