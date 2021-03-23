using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {
  Module parent;

  public Material offMaterial;
  public Material onMaterial;
  MeshRenderer meshRenderer;

  void Awake() {
    parent = GetComponentInParent<Module>();
    meshRenderer = GetComponent<MeshRenderer>();
  }

  public void SetGlow(bool glowing) {
    meshRenderer.sharedMaterial = glowing ? onMaterial : offMaterial;
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
