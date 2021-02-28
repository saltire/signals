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

  CameraUtil cameraUtil;
  LineRenderer line;
  float lineWidth = .05f;

  bool connecting = false;

  void Awake() {
    MidiMaster.knobDelegate += OnMidiKnob;
    Background.backgroundClickDelegate += OnBackgroundClick;
    Knob.knobClickDelegate += OnKnobClick;

    SetValue(MidiMaster.GetKnob(control, 0));

    cameraUtil = FindObjectOfType<CameraUtil>();

    line = GetComponent<LineRenderer>();
    line.widthMultiplier = lineWidth;
    DrawLine();
  }

  void Update() {
    DrawLine();
  }

  void DrawLine() {
    if (connecting) {
      line.positionCount = 2;
      line.SetPositions(new[] {
        transform.position,
        cameraUtil.MousePositionOnPlane(transform.position.y),
      });
    }
    else if (connectedKnob != null) {
      line.positionCount = 2;
      line.SetPositions(new[] {
        transform.position,
        connectedKnob.transform.position,
      });
    }
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
    if (connecting) {
      connecting = false;
      line.positionCount = 0;
    }
    else {
      connectedKnob = null;
      connecting = true;
    }
  }

  public void OnKnobClick(Knob knob) {
    if (connecting) {
      connecting = false;
      connectedKnob = knob;
    }
  }

  public void OnBackgroundClick() {
    if (connecting) {
      connecting = false;
      line.positionCount = 0;
    }
  }
}
