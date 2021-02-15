using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SignalInput : MonoBehaviour, IPointerClickHandler {
  ISignalNode parent;

  public SignalOutput connectedOutput;
  public float amount = 0;

  CableManager cables;

  void Start() {
    parent = GetComponentInParent<ISignalNode>();
    cables = FindObjectOfType<CableManager>();
  }

  public bool IsConnected() {
    return connectedOutput != null;
  }

  public float GetValue(double sample, Stack<ISignalNode> nodes) {
    nodes.Push(parent);

    return connectedOutput != null ? connectedOutput.GetValue(sample, nodes) * amount : 0;
  }

  public void OnPointerClick(PointerEventData data) {
    cables.OnInputClick(this);
  }
}
