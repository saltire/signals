using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MIDIInput : InputPort {
  public override PortType Type { get { return PortType.MIDI; } }

  public void OnMIDIEvent(float frequency, float volume) {
    parent.OnMIDIEvent(frequency, volume);
  }
}
