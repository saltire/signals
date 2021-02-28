using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void BackgroundClickDelegate();

public class Background : MonoBehaviour, IPointerClickHandler {
  CableManager cables;

  public static event BackgroundClickDelegate backgroundClickDelegate;

  void Awake() {
    cables = FindObjectOfType<CableManager>();
  }

  public void OnPointerClick(PointerEventData data) {
    backgroundClickDelegate();
  }
}
