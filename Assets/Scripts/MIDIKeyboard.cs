using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIDIKeyboard : MonoBehaviour {
  MIDIOutput output;

  void Awake() {
    output = GetComponentInChildren<MIDIOutput>();
  }

  public void OnKeyDown(MIDIKey key) {
    output.SendMIDIEvent(key.note, 1);
    // output.SendMIDIEvent(key.note + 4, 1);
    // output.SendMIDIEvent(key.note + 7, 1);
  }

  public void OnKeyUp(MIDIKey key) {
    output.SendMIDIEvent(key.note, 0);
    // output.SendMIDIEvent(key.note + 4, 0);
    // output.SendMIDIEvent(key.note + 7, 0);
  }
}
