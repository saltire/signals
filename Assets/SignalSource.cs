using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalSource : MonoBehaviour {
  public Oscillator[] oscillators;

  void OnAudioFilterRead(float[] data, int channels) {
    for (int i = 0; i < data.Length; i += channels) {
      foreach (Oscillator osc in oscillators) {
        data[i] += osc.GetValue();
      }

      if (channels == 2) {
        data[i + 1] = data[i];
      }
    }
  }
}
