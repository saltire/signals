using UnityEngine;
using UnityEngine.EventSystems;

public enum PortType {
  Input,
  Output,
}

public abstract class SignalPort : MonoBehaviour, IPointerClickHandler {
  public PortType type;

  protected SignalNode parent;
  protected CableManager cables;

  void Awake() {
    parent = GetComponentInParent<SignalNode>();
    cables = FindObjectOfType<CableManager>();
  }

  public void OnPointerClick(PointerEventData data) {
    cables.OnPortClick(this);
  }
}
