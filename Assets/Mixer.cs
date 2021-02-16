using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mixer : MonoBehaviour, ISignalNode {
  SignalInput[] inputs;

  void Start() {
    inputs = GetComponentsInChildren<SignalInput>();
  }

  public float GetValue(double sample, Stack<ISignalNode> nodes) {
    return inputs.Select(i => i.GetValue(sample, nodes)).Sum();
  }

  public float[] GetValues(double sample, int count, Stack<ISignalNode> nodes) {
    float[][] values = inputs.Select(i => i.GetValues(sample, count, nodes)).ToArray();

    return values[0].Zip(values[1], (a, b) => a + b).ToArray();
  }
}
