using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerClickHandler {
  public Color color = Color.red;

  SignalNode parent;

  Material material;

  void Awake() {
    material = GetComponent<MeshRenderer>().material;
    material.color = color;
    parent = GetComponentInParent<SignalNode>();
  }

  public void SetGlow(bool glowing) {
    material.SetColor("_EmissionColor", glowing ? color : Color.black);
  }

  public void OnPointerClick(PointerEventData data) {
    parent.OnButtonClick(this);
  }
}
