using UnityEngine;
using UnityEngine.EventSystems;

public enum PortType {
  Input,
  Output,
}

public abstract class SignalPort :
MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
  public PortType type;

  protected SignalNode parent;
  CableManager cables;
  Material material;

  void Awake() {
    parent = GetComponentInParent<SignalNode>();
    cables = FindObjectOfType<CableManager>();
    material = GetComponent<MeshRenderer>().material;
  }

  public void SetColor(Color color) {
    material.color = color;
  }

  public void OnPointerEnter(PointerEventData data) {
    cables.OnPortEnter(this);
  }

  public void OnPointerExit(PointerEventData data) {
    SetColor(Color.white);
  }

  public void OnPointerClick(PointerEventData data) {
    cables.OnPortClick(this);
  }
}
