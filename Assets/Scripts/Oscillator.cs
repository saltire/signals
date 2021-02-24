using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum WaveType {
  Sine,
  Square,
  Sawtooth,
  Noise,
};

public class Oscillator : SignalNode {
  public SignalInput frequencyAdjustInput;
  public SignalInput volumeAdjustInput;
  public RangeControl frequencyKnob;
  public Button sineButton;
  public Button squareButton;
  public Button sawtoothButton;
  public Button noiseButton;
  Button[] buttons;

  public float volume = 1;
  // public float volumeAdjustSensitivity = .1f;

  public WaveType wave = WaveType.Sine;

  double phase;
  double lastSample = 0;
  double sampleFrequency = 48000;
  const double TAU = Mathf.PI * 2.0;

  System.Random rand;

  void Awake() {
    rand = new System.Random();

    if (wave == WaveType.Sine) {
      sineButton.SetGlow(true);
    }
    else if (wave == WaveType.Square) {
      squareButton.SetGlow(true);
    }
    else if (wave == WaveType.Sawtooth) {
      sawtoothButton.SetGlow(true);
    }
    else if (wave == WaveType.Noise) {
      noiseButton.SetGlow(true);
    }
  }

  // public void AdjustFrequency(float speed) {
  //   // TODO: rounding options; exponentially increase speed; accelerate speed over time
  //   frequency = Mathf.Max(0, (float)frequency + speed * Time.deltaTime * frequencyAdjustSensitivity);
  // }

  // public void AdjustVolume(float speed) {
  //   volume = Mathf.Max(0, volume + speed * Time.deltaTime * volumeAdjustSensitivity);
  // }

  public override void OnButtonClick(Button button) {
    sineButton.SetGlow(false);
    squareButton.SetGlow(false);
    sawtoothButton.SetGlow(false);
    noiseButton.SetGlow(false);

    button.SetGlow(true);

    if (button == sineButton) {
      wave = WaveType.Sine;
    }
    else if (button == squareButton) {
      wave = WaveType.Square;
    }
    else if (button == sawtoothButton) {
      wave = WaveType.Sawtooth;
    }
    else if (button == noiseButton) {
      wave = WaveType.Noise;
    }
  }

  public override double GetValue(double sample, Stack<SignalNode> nodes) {
    double sampleIncrement = sample - lastSample;
    lastSample = sample;

    double currentFrequency = frequencyKnob.value;
    if (frequencyAdjustInput.IsConnected()) {
      currentFrequency += frequencyAdjustInput.GetValue(sample, nodes);
    }
    double increment = sampleIncrement * currentFrequency / sampleFrequency;

    phase = (phase + increment) % 1;

    double value = 0;

    if (wave == WaveType.Sine) {
      value = Mathf.Sin((float)(phase * TAU));
    }
    else if (wave == WaveType.Square) {
      value = Mathf.Sign(Mathf.Sin((float)(phase * TAU)));
    }
    else if (wave == WaveType.Sawtooth) {
      value = phase * 2 - 1;
    }
    else if (wave == WaveType.Noise) {
      value = rand.NextDouble() * 2 - 1;
    }

    if (volumeAdjustInput.IsConnected()) {
      value *= 1 + volumeAdjustInput.GetValue(sample, nodes);
    }

    return value * volume;
  }

  public override double[] GetValues(double sample, int count, Stack<SignalNode> nodes) {
    double[] values = new double[count];

    double[] frequencyAdjustValues = frequencyAdjustInput.IsConnected() ?
      frequencyAdjustInput.GetValues(sample, count, nodes) :
      Enumerable.Repeat(0d, count).ToArray();
    double[] volumeAdjustValues = volumeAdjustInput.IsConnected() ?
      volumeAdjustInput.GetValues(sample, count, nodes) :
      Enumerable.Repeat(0d, count).ToArray();

    float frequency = frequencyKnob.value;

    for (int i = 0; i < count; i++) {
      double thisSample = sample + i;
      double sampleIncrement = thisSample - lastSample;
      lastSample = thisSample;

      double currentFrequency = frequency + frequencyAdjustValues[i];
      double increment = sampleIncrement * currentFrequency / sampleFrequency;

      phase = (phase + increment) % 1;

      double value = 0;

      if (wave == WaveType.Sine) {
        value = Mathf.Sin((float)(phase * TAU));
      }
      else if (wave == WaveType.Square) {
        value = Mathf.Sign(Mathf.Sin((float)(phase * TAU)));
      }
      else if (wave == WaveType.Sawtooth) {
        value = phase * 2 - 1;
      }
      else if (wave == WaveType.Noise) {
        value = rand.NextDouble() * 2 - 1;
      }

      values[i] = value * (1 + volumeAdjustValues[i]) * volume;
    }

    return values;
  }
}
