using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour {
  SignalInput input;

  [Range(0, 10)]
  public float volume = 1;

  double sample = 0;

  void Start() {
    input = GetComponentInChildren<SignalInput>();
  }

  void OnAudioFilterRead(float[] data, int channels) {
    int count = data.Length / channels;
    float volumeMultiplier = volume / 10;

    double[] values = input.GetValues(sample, count, new Stack<ISignalNode>());

    for (int i = 0; i < count; i++) {
      for (int c = 0; c < channels; c++) {
        data[i * channels + c] = (float)values[i] * volumeMultiplier;
      }
    }

    sample += count;
  }
}