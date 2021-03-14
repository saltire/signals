using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KnobClickDelegate(Knob knob);

public class Knob : RangeControl, IBeginDragHandler, IDragHandler, IPointerClickHandler {
  public float sensitivity = .1f; // Percent the dial will move per 1 unit the pointer is dragged.

  public static event KnobClickDelegate knobClickDelegate;

  CameraUtil cameraUtil;
  Vector3 beginDragPosition;
  float beginDragValue;

  float minAngle = -215;
  float maxAngle = 45;

  void Awake() {
    cameraUtil = FindObjectOfType<CameraUtil>();

    Rotate();
  }

  void Rotate() {
    transform.rotation = Quaternion.Euler(0, Mathf.Lerp(minAngle, maxAngle, value), 0);
  }

  public void SetValue(float newValue) {
    value = Mathf.Clamp01(newValue);
    Rotate();
  }

  public void OnBeginDrag(PointerEventData data) {
    beginDragPosition = cameraUtil.MousePositionOnPlane(data.pressPosition, transform.position.y);
    beginDragValue = value;
  }

  public void OnDrag(PointerEventData data) {
    Vector3 dragPosition = cameraUtil.MousePositionOnPlane(data.position, transform.position.y);
    float deltaValue = (dragPosition.x - beginDragPosition.x) * sensitivity;
    SetValue(beginDragValue + deltaValue);
  }

  public void OnPointerClick(PointerEventData data) {
    knobClickDelegate(this);
  }
}
