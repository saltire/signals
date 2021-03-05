using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateInput : InputPort {
  public override PortType Type { get { return PortType.Gate; } }

  public void OnGateTrigger(bool open) {
    parent.OnGateTrigger(open);
  }
}
