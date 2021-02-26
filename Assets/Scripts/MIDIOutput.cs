using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MIDIOutput : OutputPort, IMIDIPort {
  public override PortType Type { get { return PortType.MIDI; } }
}
