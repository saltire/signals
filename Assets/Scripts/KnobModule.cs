using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnobModule : SignalModule {
  RangeControl knob;

  void Awake() {
    knob = GetComponentInChildren<RangeControl>();
  }

  public override double[] GetValues(double sample, int count, Stack<SignalModule> modules,
    SignalOutput output) {
    double[] values = new double[count];
    for (int i = 0; i < count; i++) {
      values[i] = knob.value;
    }
    return values;
  }
}
