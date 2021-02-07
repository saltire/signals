using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WaveType {
  Sine,
  Square,
  Sawtooth,
  Noise,
};

public class Oscillator : MonoBehaviour {
  const double TAU = Mathf.PI * 2.0;

  public double frequency = 440;
  double increment;
  double phase;
  double sampleFrequency = 48000;

  public WaveType wave = WaveType.Sine;

  public float gain;
  public float volume = 0.1f;

  public float[] frequencies = { 440, 494, 554, 587, 659, 740, 831, 880 };
  int freqIndex = 0;

  System.Random rand;

  void Awake() {
    rand = new System.Random();
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) {
      if (gain == 0) {
        gain = volume;
        frequency = frequencies[freqIndex];
        freqIndex = (freqIndex + 1) % frequencies.Length;
      }
      else {
        gain = 0;
      }
    }
  }

  void OnAudioFilterRead(float[] data, int channels) {
    increment = frequency * TAU / sampleFrequency;

    for (int i = 0; i < data.Length; i += channels) {
      phase = (phase + increment) % TAU;

      float value = 0;
      if (wave == WaveType.Sine) {
        value = Mathf.Sin((float)phase);
      }
      else if (wave == WaveType.Square) {
        value = Mathf.Sign(Mathf.Sin((float)phase));
      }
      else if (wave == WaveType.Sawtooth) {
        value = (float)(phase / TAU) * 2 - 1;
      }
      else if (wave == WaveType.Noise) {
        value = (float)rand.NextDouble() * 2 - 1;
      }

      data[i] += (float)(value * gain);

      if (channels == 2) {
        data[i + 1] = data[i];
      }
    }
  }
}
