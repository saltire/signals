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
}
