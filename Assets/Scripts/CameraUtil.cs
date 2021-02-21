using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtil : MonoBehaviour {
  Camera mainCamera;

  void Awake() {
    mainCamera = GetComponent<Camera>();
  }

  public Vector3 MousePositionOnPlane(Vector2 mousePosition, float y) {
    Plane plane = new Plane(Vector3.down, Vector3.up * y);
    Ray mouseRay = mainCamera.ScreenPointToRay(mousePosition);
    float distance;
    plane.Raycast(mouseRay, out distance);
    return mouseRay.GetPoint(distance);
  }
}
