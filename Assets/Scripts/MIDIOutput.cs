using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MIDIOutput : OutputPort {
  public override PortType Type { get { return PortType.MIDI; } }

  public void OnMIDIEvent(int note, float velocity) {
    if (IsConnected()) {
      ((MIDIInput)connectedInput).OnMIDIEvent(note, velocity);
    }
  }
}
