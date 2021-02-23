using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mixer : SignalNode {
  SignalInput[] inputs;
  Knob fader;

  void Awake() {
    inputs = GetComponentsInChildren<SignalInput>();
    fader = GetComponentInChildren<Knob>();
  }

  float[] GetMultipliers() {
    return new float[] {
      Mathf.Min((2 - fader.value) * 2, 1),
      Mathf.Min((fader.value - 1) * 2, 1),
    };
  }

  public override double GetValue(double sample, Stack<SignalNode> nodes) {
    float[] mult = GetMultipliers();

    return inputs.Select((input, i) => input.GetValue(sample, nodes) * mult[i]).Sum();
  }

  public override double[] GetValues(double sample, int count, Stack<SignalNode> nodes) {
    double[][] values = inputs.Select(i => i.GetValues(sample, count, nodes)).ToArray();
    float[] mult = GetMultipliers();

    return values[0].Zip(values[1], (v0, v1) => v0 * mult[0] + v1 * mult[1]).ToArray();
  }
}
