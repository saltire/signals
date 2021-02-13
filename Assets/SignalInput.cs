using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalInput : MonoBehaviour {
  public SignalOutput connectedOutput;
  public float amount = 0;

  public bool IsConnected() {
    return connectedOutput != null;
  }

  public float GetValue() {
    return connectedOutput != null ? connectedOutput.GetValue() * amount : 0;
  }
}
