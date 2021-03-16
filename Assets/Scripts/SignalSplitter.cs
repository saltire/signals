using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalSplitter : SignalModule {
  public SignalInput input;

  void Awake() {
    input = GetComponentInChildren<SignalInput>();
  }

  public override double[] GetValues(double sample, int count, Stack<SignalModule> modules,
    SignalOutput output) {
    // TODO: cache last fetched values by sample index.
    return input.GetValues(sample, count, modules);
  }
}
