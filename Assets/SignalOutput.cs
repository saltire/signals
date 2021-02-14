using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SignalOutput : MonoBehaviour, IPointerClickHandler {
  // Eventually will be able to map this to different values in its parent.
  // For now just assume oscillator output.
  Oscillator osc;

  CableManager cables;

  void Start() {
    osc = GetComponentInParent<Oscillator>();
    cables = FindObjectOfType<CableManager>();
  }

  public float GetValue() {
    return osc.GetValue();
  }

  public void OnPointerClick(PointerEventData data) {
    cables.OnOutputClick(this);
  }
}
