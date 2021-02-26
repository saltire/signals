using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MIDIOutput : OutputPort {
  public override PortType Type { get { return PortType.MIDI; } }

  public void SendMIDIEvent(float frequency, float volume) {
    if (IsConnected()) {
      ((MIDIInput)connectedInput).OnMIDIEvent(frequency, volume);
    }
  }
}
