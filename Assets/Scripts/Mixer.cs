using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MixType {
  Add,
  Multiply,
}

public class Mixer : SignalModule {
  SignalInput[] inputs;
  RangeControl fader;
  public Button addButton;
  public Button multiplyButton;

  public MixType mixType = MixType.Add;

  void Awake() {
    inputs = GetComponentsInChildren<SignalInput>();
    fader = GetComponentInChildren<RangeControl>();

    if (mixType == MixType.Add) {
      addButton.SetGlow(true);
    }
    else if (mixType == MixType.Multiply) {
      multiplyButton.SetGlow(true);
    }
  }

  public override void OnButtonClick(Button button) {
    button.SetGlow(true);

    if (button == addButton) {
      mixType = MixType.Add;
      multiplyButton.SetGlow(false);
    }
    else if (button == multiplyButton) {
      mixType = MixType.Multiply;
      addButton.SetGlow(false);
    }
  }

  float[] GetMultipliers() {
    // Fader range should go from -1 to 1.
    return new[] {
      Mathf.InverseLerp(1, 0, fader.value),
      Mathf.InverseLerp(-1, 0, fader.value),
    };
  }

  public override double[] GetValues(double sample, int count, Stack<SignalModule> modules) {
    double[][] values = inputs.Select(i => i.GetValues(sample, count, modules)).ToArray();
    float[] mult = GetMultipliers();

    return values[0].Zip(values[1], (v0, v1) => {
      double mv0 = v0 * mult[0];
      double mv1 = v1 * mult[1];
      return mixType == MixType.Multiply ? mv0 * mv1 : mv0 + mv1;
    }).ToArray();
  }
}
