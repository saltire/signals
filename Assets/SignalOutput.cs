using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SignalOutput : MonoBehaviour, IPointerClickHandler {
  ISignalNode parent;

  CableManager cables;

  void Start() {
    parent = GetComponentInParent<ISignalNode>();
    cables = FindObjectOfType<CableManager>();
  }

  public float GetValue(double sample, Stack<ISignalNode> nodes) {
    // Return zero if the signal has already passed through this node, to avoid infinite loops.
    if (nodes.Contains(parent)) {
      return 0;
    }

    return parent.GetValue(sample, nodes);
  }

  public void OnPointerClick(PointerEventData data) {
    cables.OnOutputClick(this);
  }
}
