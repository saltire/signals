using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MIDIInput : InputPort {
  public override PortType Type { get { return PortType.MIDI; } }

  public void OnMIDIEvent(int note, float volume) {
    parent.OnMIDIEvent(note, volume);
  }
}
