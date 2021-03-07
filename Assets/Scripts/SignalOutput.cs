using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SignalOutput : OutputPort {
  public override PortType Type { get { return PortType.Signal; } }

  public double[] GetValues(double sample, int count, Stack<SignalModule> modules) {
    if (modules.Contains(parent)) {
      return Enumerable.Repeat(0d, count).ToArray();
    }

    return parent.GetValues(sample, count, modules, this);
  }
}
