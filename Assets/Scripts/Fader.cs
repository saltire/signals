using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fader : RangeControl, IBeginDragHandler, IDragHandler {
  public float min = -1;
  public float max = 1;
  public float sensitivity = 1;
  public float[] snapValues = new[] { 0f };
  public float snapRange = .1f;

  CameraUtil cameraUtil;
  Vector3 beginDragPosition;
  float beginDragValue;

  Transform slot;
  Transform handle;

  void Awake() {
    cameraUtil = FindObjectOfType<CameraUtil>();

    slot = GetComponentInChildren<SpriteRenderer>().transform;
    handle = GetComponentInChildren<Collider>().transform;

    MoveHandle();
  }

  void MoveHandle() {
    float valPercent = Mathf.InverseLerp(min, max, value);
    handle.position = new Vector3(slot.position.x + slot.localScale.x * (valPercent - .5f),
      handle.position.y, handle.position.z);
  }

  public void OnBeginDrag(PointerEventData data) {
    beginDragPosition = cameraUtil.MousePositionOnPlane(data.pressPosition, transform.position.y);
    beginDragValue = value;
  }

  public void OnDrag(PointerEventData data) {
    Vector3 dragPosition = cameraUtil.MousePositionOnPlane(data.position, transform.position.y);
    float deltaValue = (dragPosition.x - beginDragPosition.x) * sensitivity;
    value = Mathf.Clamp(beginDragValue + deltaValue, min, max);

    foreach (float snapValue in snapValues) {
      if (Mathf.Abs(value - snapValue) < snapRange) {
        value = snapValue;
        break;
      }
    }

    MoveHandle();
  }
}
