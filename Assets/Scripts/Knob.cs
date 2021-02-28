using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KnobClickDelegate(Knob knob);

public class Knob : RangeControl, IBeginDragHandler, IDragHandler, IPointerClickHandler {
  public float min = 0;
  public float max = 100;
  public float sensitivity = 1;

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
    float valPercent = Mathf.InverseLerp(min, max, value);
    transform.rotation = Quaternion.Euler(0, Mathf.Lerp(minAngle, maxAngle, valPercent), 0);
  }

  public void SetValue(float newValue) {
    value = newValue;
    Rotate();
  }

  public void SetValuePercent(float valPercent) {
    value = Mathf.Lerp(min, max, valPercent);
    Rotate();
  }

  public void OnBeginDrag(PointerEventData data) {
    beginDragPosition = cameraUtil.MousePositionOnPlane(data.pressPosition, transform.position.y);
    beginDragValue = value;
  }

  public void OnDrag(PointerEventData data) {
    Vector3 dragPosition = cameraUtil.MousePositionOnPlane(data.position, transform.position.y);
    float deltaValue = (dragPosition.x - beginDragPosition.x) * sensitivity;
    SetValue(Mathf.Clamp(beginDragValue + deltaValue, min, max));
  }

  public void OnPointerClick(PointerEventData data) {
    knobClickDelegate(this);
  }
}
