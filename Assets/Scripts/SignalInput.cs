using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SignalInput : InputPort {
  public override PortType Type { get { return PortType.Signal; } }

  public float amount = 0;

  public double[] GetValues(double sample, int count, Stack<SignalModule> modules) {
    modules.Push((SignalModule)parent);

    return connectedOutput == null ? Enumerable.Repeat(0d, count).ToArray() :
      ((SignalOutput)connectedOutput)
        .GetValues(sample, count, modules).Select(v => v * amount).ToArray();
  }
}
