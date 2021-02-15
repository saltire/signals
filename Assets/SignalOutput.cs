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

  public float GetValue(double sample) {
    return parent.GetValue(sample);
  }

  public void OnPointerClick(PointerEventData data) {
    cables.OnOutputClick(this);
  }
}
