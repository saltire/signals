using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PortSide {
  Input,
  Output,
}

public enum PortType {
  Gate,
  MIDI,
  Signal,
}

public abstract class Port : MonoBehaviour,
IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
  public abstract PortSide Side { get; }
  public abstract PortType Type { get; }

  protected Module parent;
  CableManager cables;
  MeshRenderer meshRenderer;

  void Awake() {
    parent = GetComponentInParent<Module>();
    cables = FindObjectOfType<CableManager>();
    meshRenderer = GetComponent<MeshRenderer>();
  }

  public abstract bool IsConnected();

  public void SetMaterial(Material material) {
    meshRenderer.sharedMaterial = material;
  }

  public void OnPointerEnter(PointerEventData data) {
    cables.OnPortEnter(this);
  }

  public void OnPointerExit(PointerEventData data) {
    cables.OnPortExit(this);
  }

  public void OnPointerClick(PointerEventData data) {
    cables.OnPortClick(this);
  }
}

public abstract class InputPort : Port {
  public override PortSide Side { get { return PortSide.Input; } }

  public OutputPort connectedOutput;

  public override bool IsConnected() {
    return connectedOutput != null;
  }
}

public abstract class OutputPort : Port {
  public override PortSide Side { get { return PortSide.Output; } }

  public InputPort connectedInput;

  public override bool IsConnected() {
    return connectedInput != null;
  }
}
