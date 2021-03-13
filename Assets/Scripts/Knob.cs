using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KnobClickDelegate(Knob knob);

public class Knob : RangeControl, IBeginDragHandler, IDragHandler, IPointerClickHandler {
  public float min = 0;
  public float max = 100;
  public float sensitivity = .1f; // Percent the dial will move per 1 unit the pointer is dragged.

  public static event KnobClickDelegate knobClickDelegate;

  CameraUtil cameraUtil;
  Vector3 beginDragPosition;
  float beginDragValuePercent;

  float minAngle = -215;
  float maxAngle = 45;

  void Awake() {
    cameraUtil = FindObjectOfType<CameraUtil>();

    Rotate();
  }

  void Rotate() {
    float valPercent = Mathf.InverseLerp(min, max, value);
    transform.rotation = Quaternion.Euler(0, Mathf.Lerp(minAngle, maxAngle, valPercent), 0);
  }

  void SetValue(float newValue) {
    value = newValue;
    Rotate();
  }

  public void SetValuePercent(float valPercent) {
    value = Mathf.Lerp(min, max, valPercent);
    Rotate();
  }

  public void OnBeginDrag(PointerEventData data) {
    beginDragPosition = cameraUtil.MousePositionOnPlane(data.pressPosition, transform.position.y);
    beginDragValuePercent = Mathf.InverseLerp(min, max, value);
  }

  public void OnDrag(PointerEventData data) {
    Vector3 dragPosition = cameraUtil.MousePositionOnPlane(data.position, transform.position.y);
    float deltaValuePercent = (dragPosition.x - beginDragPosition.x) * sensitivity;
    SetValuePercent(beginDragValuePercent + deltaValuePercent);
  }

  public void OnPointerClick(PointerEventData data) {
    knobClickDelegate(this);
  }
}
