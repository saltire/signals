using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour {
  SignalInput input;
  Knob volume;

  double sample = 0;

  void Awake() {
    input = GetComponentInChildren<SignalInput>();
    volume = GetComponentInChildren<Knob>();
  }

  void OnAudioFilterRead(float[] data, int channels) {
    int count = data.Length / channels;
    float volumeMultiplier = volume.value / 10;

    double[] values = input.GetValues(sample, count, new Stack<ISignalNode>());

    for (int i = 0; i < count; i++) {
      for (int c = 0; c < channels; c++) {
        data[i * channels + c] = (float)values[i] * volumeMultiplier;
      }
    }

    sample += count;
  }
}
