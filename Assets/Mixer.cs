using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mixer : MonoBehaviour, ISignalNode {
  SignalInput[] inputs;

  [Range(1, 2)]
  public float fader = 1.5f;

  void Start() {
    inputs = GetComponentsInChildren<SignalInput>();
  }

  float[] GetMultipliers() {
    return new float[] {
      Mathf.Min((2 - fader) * 2, 1),
      Mathf.Min((fader - 1) * 2, 1),
    };
  }

  public double GetValue(double sample, Stack<ISignalNode> nodes) {
    float[] mult = GetMultipliers();

    return inputs.Select((input, i) => input.GetValue(sample, nodes) * mult[i]).Sum();
  }

  public double[] GetValues(double sample, int count, Stack<ISignalNode> nodes) {
    double[][] values = inputs.Select(i => i.GetValues(sample, count, nodes)).ToArray();
    float[] mult = GetMultipliers();

    return values[0].Zip(values[1], (v0, v1) => v0 * mult[0] + v1 * mult[1]).ToArray();
  }
}
