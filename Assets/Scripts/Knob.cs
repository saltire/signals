using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Knob : MonoBehaviour, IBeginDragHandler, IDragHandler {
  public float min = 0;
  public float max = 100;
  public float sensitivity = 1;

  public float value = 0;

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
    float val = Mathf.InverseLerp(min, max, value);
    transform.rotation = Quaternion.Euler(0, Mathf.Lerp(minAngle, maxAngle, val), 0);
  }

  public void OnBeginDrag(PointerEventData data) {
    beginDragPosition = cameraUtil.MousePositionOnPlane(data.pressPosition, transform.position.y);
    beginDragValue = value;
  }

  public void OnDrag(PointerEventData data) {
    Vector3 dragPosition = cameraUtil.MousePositionOnPlane(data.position, transform.position.y);
    float deltaValue = (dragPosition.x - beginDragPosition.x) * sensitivity;
    value = Mathf.Clamp(beginDragValue + deltaValue, min, max);
    Rotate();
  }
}
