using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour {
  SignalInput input;

  void Start() {
    input = GetComponentInChildren<SignalInput>();
  }

  void OnAudioFilterRead(float[] data, int channels) {
    for (int i = 0; i < data.Length; i += channels) {
      data[i] = input.GetValue();

      if (channels == 2) {
        data[i + 1] = data[i];
      }
    }
  }
}
