using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSplitter : Module {
  GateOutput[] outputs;

  void Awake() {
    outputs = GetComponentsInChildren<GateOutput>();
  }

  public override void OnGateTrigger(bool open) {
    foreach (GateOutput output in outputs) {
      output.OnGateTrigger(open);
    }
  }
}
