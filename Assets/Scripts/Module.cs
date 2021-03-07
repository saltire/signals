using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour {
  public virtual void OnButtonClick(Button button) {}
  public virtual void OnButtonDown(Button button) {}
  public virtual void OnButtonUp(Button button) {}
  public virtual void OnGateTrigger(bool on) {}
  public virtual void OnMIDIEvent(int note, float volume) {}
}

public abstract class SignalModule : Module {
  public abstract double[] GetValues(double sample, int count, Stack<SignalModule> modules,
    SignalOutput output);
}
