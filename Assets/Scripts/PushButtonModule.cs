using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButtonModule : Module {
  GateOutput output;

  public float pushDistance = .1f;

  void Awake() {
    output = GetComponentInChildren<GateOutput>();
  }

  public override void OnButtonDown(Button button) {
    output.OnGateTrigger(true);
    button.transform.position += Vector3.down * pushDistance;
  }

  public override void OnButtonUp(Button button) {
    output.OnGateTrigger(false);
    button.transform.position += Vector3.up * pushDistance;
  }
}
