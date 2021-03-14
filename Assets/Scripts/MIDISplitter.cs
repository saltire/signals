using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIDISplitter : Module {
  MIDIOutput[] outputs;

  void Awake() {
    outputs = GetComponentsInChildren<MIDIOutput>();
  }

  public override void OnMIDIEvent(int note, float velocity) {
    foreach (MIDIOutput output in outputs) {
      output.OnMIDIEvent(note, velocity);
    }
  }
}
