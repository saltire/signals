using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SignalOutput : OutputPort, ISignalPort {
  public override PortType Type { get { return PortType.Signal; } }

  public double[] GetValues(double sample, int count, Stack<SignalNode> nodes) {
    if (nodes.Contains(parent)) {
      return Enumerable.Repeat(0d, count).ToArray();
    }

    return parent.GetValues(sample, count, nodes);
  }
}
