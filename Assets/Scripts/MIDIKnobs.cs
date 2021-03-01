using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIDIKnobs : Module {
  Button button;
  LineRenderer[] knobLines;

  public bool showLines = true;

  void Awake() {
    button = GetComponentInChildren<Button>();
    knobLines = GetComponentsInChildren<LineRenderer>();
  }

  void Start() {
    button.SetGlow(showLines);
  }

  public override void OnButtonClick(Button button) {
    showLines = !showLines;
    button.SetGlow(showLines);

    foreach (LineRenderer knobLine in knobLines) {
      knobLine.enabled = showLines;
    }
  }
}
