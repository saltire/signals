using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public enum Chord {
  None,
  Major,
  Minor,
}

public class MIDIKeyboard : MonoBehaviour {
  MIDIOutput output;

  public Chord chord = Chord.None;

  void Awake() {
    output = GetComponentInChildren<MIDIOutput>();

    MidiMaster.noteOnDelegate += OnMidiNoteOn;
    MidiMaster.noteOffDelegate += OnMidiNoteOff;
  }

  void OnMidiNoteOn(MidiChannel channel, int note, float velocity) {
    SendNote(note, velocity);
  }

  void OnMidiNoteOff(MidiChannel channel, int note) {
    SendNote(note, 0);
  }

  public void OnKeyDown(MIDIKey key) {
    SendNote(key.note, 1);
  }

  public void OnKeyUp(MIDIKey key) {
    SendNote(key.note, 0);
  }

  void SendNote(int note, float velocity) {
    output.OnMIDIEvent(note, velocity);

    if (chord == Chord.Major) {
      output.OnMIDIEvent(note + 4, velocity);
      output.OnMIDIEvent(note + 7, velocity);
    }
    else if (chord == Chord.Minor) {
      output.OnMIDIEvent(note + 3, velocity);
      output.OnMIDIEvent(note + 7, velocity);
    }
  }
}
