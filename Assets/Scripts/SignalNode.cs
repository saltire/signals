using System.Collections.Generic;
using UnityEngine;

public abstract class SignalNode : MonoBehaviour {
  public virtual void OnButtonClick(Button button) {}
  public virtual void OnMIDIEvent(float frequency, float volume) {}

  public abstract double[] GetValues(double sample, int count, Stack<SignalNode> nodes);
}
