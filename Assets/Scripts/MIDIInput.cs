using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MIDIInput : InputPort, IMIDIPort {
  public override PortType Type { get { return PortType.MIDI; } }
}
