using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PortSide {
  Input,
  Output,
}

public enum PortType {
  Signal,
  MIDI,
}

public abstract class Port : MonoBehaviour,
IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
  public abstract PortSide Side { get; }
  public abstract PortType Type { get; }

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

// Sides

public abstract class InputPort : Port {
  public override PortSide Side { get { return PortSide.Input; } }

  public OutputPort connectedOutput;

  public bool IsConnected() {
    return connectedOutput != null;
  }
}

public abstract class OutputPort : Port {
  public override PortSide Side { get { return PortSide.Output; } }
}

// Types

public interface ISignalPort {
  public double[] GetValues(double sample, int count, Stack<SignalNode> nodes);
}

public interface IMIDIPort {}
