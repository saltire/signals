using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SignalInput : SignalPort {
  public SignalOutput connectedOutput;
  public float amount = 0;

  public bool IsConnected() {
    return connectedOutput != null;
  }

  public double GetValue(double sample, Stack<ISignalNode> nodes) {
    nodes.Push(parent);

    return connectedOutput != null ? connectedOutput.GetValue(sample, nodes) * amount : 0;
  }

  public double[] GetValues(double sample, int count, Stack<ISignalNode> nodes) {
    nodes.Push(parent);

    return connectedOutput == null ? Enumerable.Repeat(0d, count).ToArray() :
      connectedOutput.GetValues(sample, count, nodes).Select(v => v * amount).ToArray();
  }
}
