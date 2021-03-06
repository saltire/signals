using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleMove : MonoBehaviour, IDragHandler {
  CameraUtil cameraUtil;

  void Awake() {
    cameraUtil = FindObjectOfType<CameraUtil>();
  }

  public void OnDrag(PointerEventData data) {
    Vector3 delta = cameraUtil.MousePositionOnPlane(data.position, transform.position.y) -
      cameraUtil.MousePositionOnPlane(data.position - data.delta, transform.position.y);

    if (!Physics.BoxCast(transform.position, transform.lossyScale / 2, delta, transform.rotation,
      delta.magnitude)) {
      transform.parent.position += delta;
    }
  }
}
