using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvelopeGenerator : SignalModule {
  public RangeControl volumeKnob;
  GateInput input;
  Envelope envelope;

  double lastSample = 0;
  double sampleFrequency = 48000;
  double? openSample;
  double? closeSample;

  void Awake() {
    input = GetComponentInChildren<GateInput>();
    envelope = GetComponentInChildren<Envelope>();
  }

  public override void OnGateTrigger(bool open) {
    if (open) {
      openSample = lastSample;
      closeSample = null;
    }
    else {
      closeSample = lastSample;
    }
  }

  public override double[] GetValues(double sample, int count, Stack<SignalModule> modules) {
    double[] values = new double[count];
    for (int i = 0; i < count; i++) {
      double thisSample = sample + i;
      lastSample = thisSample;

      values[i] = 0;

      if (openSample != null) {
        float? envVolume = envelope.GetVolume(
          (float)openSample, closeSample, thisSample, sampleFrequency);

        if (envVolume != null) {
          values[i] = (float)envVolume * volumeKnob.value;
        }
        else {
          openSample = null;
        }
      }
    }

    return values;
  }
}
