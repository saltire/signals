using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SignalInput : MonoBehaviour, IPointerClickHandler {
  ISignalNode parent;

  public SignalOutput connectedOutput;
  public float amount = 0;

  CableManager cables;

  void Start() {
    parent = GetComponentInParent<ISignalNode>();
    cables = FindObjectOfType<CableManager>();
  }

  public bool IsConnected() {
    return connectedOutput != null;
  }

  public double GetValue(double sample, Stack<ISignalNode> nodes) {
    nodes.Push(parent);

    return connectedOutput != null ? connectedOutput.GetValue(sample, nodes) * amount : 0;
  }

  public double[] GetValues(double sample, int count, Stack<ISignalNode> nodes) {
    nodes.Push(parent);

    return connectedOutput == null ? Enumerable.Repeat(0d, count).ToArray() :
      connectedOutput.GetValues(sample, count, nodes).Select(v => v * amount).ToArray();
  }

  public void OnPointerClick(PointerEventData data) {
    cables.OnInputClick(this);
  }
}
