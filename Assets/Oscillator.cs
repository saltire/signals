using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Oscillator : MonoBehaviour {
  const double TAU = Mathf.PI * 2.0;

  public double frequency = 440;
  double increment;
  double phase;
  double sampleFrequency = 48000;

  public float gain;
  public float volume = 0.1f;

  public float[] frequencies = { 440, 494, 554, 587, 659, 740, 831, 880 };
  int freqIndex = 0;

  void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) {
      gain = volume;
      frequency = frequencies[freqIndex];
      freqIndex = (freqIndex + 1) % frequencies.Length;
    }
    if (Input.GetKeyUp(KeyCode.Space)) {
      gain = 0;
    }
  }

  void OnAudioFilterRead(float[] data, int channels) {
    increment = frequency * TAU / sampleFrequency;

    for (int i = 0; i < data.Length; i += channels) {
      phase = (phase + increment) % TAU;

      data[i] = (float)(gain * Mathf.Sin((float)phase));

      if (channels == 2) {
        data[i + 1] = data[i];
      }
    }
  }
}
