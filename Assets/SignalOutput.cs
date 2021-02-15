using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SignalOutput : MonoBehaviour, IPointerClickHandler {
  ISignalNode parent;

  CableManager cables;

  void Start() {
    parent = GetComponentInParent<ISignalNode>();
    cables = FindObjectOfType<CableManager>();
  }

  public float GetValue() {
    return parent.GetValue();
  }

  public void OnPointerClick(PointerEventData data) {
    cables.OnOutputClick(this);
  }
}
