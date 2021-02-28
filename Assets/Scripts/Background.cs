using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Background : MonoBehaviour, IPointerClickHandler {
  CableManager cables;

  void Awake() {
    cables = FindObjectOfType<CableManager>();
  }

  public void OnPointerClick(PointerEventData data) {
    cables.OnBackgroundClick();
  }
}
