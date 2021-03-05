using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {
  public Color color = Color.red;

  Module parent;

  Material material;

  void Awake() {
    material = GetComponent<MeshRenderer>().material;
    material.color = color;
    parent = GetComponentInParent<Module>();
  }

  public void SetGlow(bool glowing) {
    material.SetColor("_EmissionColor", glowing ? color : Color.black);
  }

  public void OnPointerClick(PointerEventData data) {
    parent.OnButtonClick(this);
  }

  public void OnPointerDown(PointerEventData data) {
    parent.OnButtonDown(this);
  }

  public void OnPointerUp(PointerEventData data) {
    parent.OnButtonUp(this);
  }
}
