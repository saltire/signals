using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOutput : OutputPort {
  public override PortType Type { get { return PortType.Gate; } }

  public void OnGateTrigger(bool open) {
    if (IsConnected()) {
      ((GateInput)connectedInput).OnGateTrigger(open);
    }
  }
}
