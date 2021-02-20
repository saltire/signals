using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SignalOutput : SignalPort {
  public double GetValue(double sample, Stack<ISignalNode> nodes) {
    // Return zero if the signal has already passed through this node, to avoid infinite loops.
    if (nodes.Contains(parent)) {
      return 0;
    }

    return parent.GetValue(sample, nodes);
  }

  public double[] GetValues(double sample, int count, Stack<ISignalNode> nodes) {
    if (nodes.Contains(parent)) {
      return Enumerable.Repeat(0d, count).ToArray();
    }

    return parent.GetValues(sample, count, nodes);
  }
}
