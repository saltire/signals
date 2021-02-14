using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SignalInput : MonoBehaviour, IPointerClickHandler {
  public SignalOutput connectedOutput;
  public float amount = 0;

  CableManager cables;

  void Start() {
    cables = FindObjectOfType<CableManager>();
  }

  public bool IsConnected() {
    return connectedOutput != null;
  }

  public float GetValue() {
    return connectedOutput != null ? connectedOutput.GetValue() * amount : 0;
  }

  public void OnPointerClick(PointerEventData data) {
    cables.OnInputClick(this);
  }
}
