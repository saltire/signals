using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalOutput : MonoBehaviour {
  // Eventually will be able to map this to different values in its parent.
  // For now just assume oscillator output.
  Oscillator osc;

  void Start() {
    osc = GetComponentInParent<Oscillator>();
  }

  public float GetValue() {
    return osc.GetValue();
  }
}
