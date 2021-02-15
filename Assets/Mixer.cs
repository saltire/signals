using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mixer : MonoBehaviour, ISignalNode {
  SignalInput[] inputs;

  void Start() {
    inputs = GetComponentsInChildren<SignalInput>();
  }

  public float GetValue() {
    return inputs.Select(i => i.GetValue()).Sum();
  }
}
