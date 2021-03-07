using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrackpadHandle : MonoBehaviour, IDragHandler {
  Trackpad parent;
  CameraUtil cameraUtil;

  void Awake() {
    parent = GetComponentInParent<Trackpad>();
    cameraUtil = FindObjectOfType<CameraUtil>();
  }

  public void OnDrag(PointerEventData data) {
    Vector3 dragPosition = cameraUtil.MousePositionOnPlane(data.position, transform.position.y);

    transform.position = new Vector3(
      Mathf.Clamp(dragPosition.x,
        parent.grid.bounds.min.x, parent.grid.bounds.max.x),
      transform.position.y,
      Mathf.Clamp(dragPosition.z,
        parent.grid.bounds.min.z, parent.grid.bounds.max.z));
  }
}
