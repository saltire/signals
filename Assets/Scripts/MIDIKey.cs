using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MIDIKey : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
  public float frequency = 440;

  MIDIKeyboard parent;

  void Awake() {
    parent = GetComponentInParent<MIDIKeyboard>();
  }

  public void OnPointerDown(PointerEventData data) {
    parent.OnKeyDown(this);
  }

  public void OnPointerUp(PointerEventData data) {
    parent.OnKeyUp(this);
  }
}
