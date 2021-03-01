using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mixer : SignalModule {
  SignalInput[] inputs;
  RangeControl fader;

  void Awake() {
    inputs = GetComponentsInChildren<SignalInput>();
    fader = GetComponentInChildren<RangeControl>();
  }

  float[] GetMultipliers() {
    // Fader range should go from -1 to 1.
    return new float[] {
      Mathf.Min(-fader.value + 1, 1),
      Mathf.Min(fader.value + 1, 1),
    };
  }

  public override double[] GetValues(double sample, int count, Stack<SignalModule> modules) {
    double[][] values = inputs.Select(i => i.GetValues(sample, count, modules)).ToArray();
    float[] mult = GetMultipliers();

    return values[0].Zip(values[1], (v0, v1) => v0 * mult[0] + v1 * mult[1]).ToArray();
  }
}
