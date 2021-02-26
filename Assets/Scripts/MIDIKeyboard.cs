using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIDIKeyboard : MonoBehaviour {
  MIDIOutput output;

  void Awake() {
    output = GetComponentInChildren<MIDIOutput>();
  }

  public void OnKeyDown(MIDIKey key) {
    output.SendMIDIEvent(key.frequency, 1);
  }

  public void OnKeyUp(MIDIKey key) {
    output.SendMIDIEvent(key.frequency, 0);
  }
}
