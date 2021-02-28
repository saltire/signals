using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MidiJack;

public class MIDIKnob : MonoBehaviour, IPointerClickHandler {
  public int control;
  public Knob connectedKnob;

  float value = 0;
  float minAngle = -215;
  float maxAngle = 45;

  void Awake() {
    MidiMaster.knobDelegate += OnMidiKnob;

    SetValue(MidiMaster.GetKnob(control, 0));
  }

  void SetValue(float newValue) {
    value = newValue;
    transform.rotation = Quaternion.Euler(0, Mathf.Lerp(minAngle, maxAngle, value), 0);

    if (connectedKnob != null) {
      connectedKnob.SetValuePercent(newValue);
    }
  }

  void OnMidiKnob(MidiChannel channel, int knobNumber, float knobValue) {
    if (knobNumber == control) {
      SetValue(knobValue);
    }
  }

  public void OnPointerClick(PointerEventData data) {

  }
}
