using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Envelope : MonoBehaviour {
  public RangeControl attackKnob;
  public RangeControl decayKnob;
  public RangeControl sustainKnob;
  public RangeControl releaseKnob;

  float attack = 0;
  float decay = 0;
  float sustain = 1;
  float release = 0;

  Oscillator parent;
  LineRenderer line;

  public float xScale = .25f;
  public float lineWidth = .01f;

  void Awake() {
    parent = GetComponentInParent<Oscillator>();
    line = GetComponentInChildren<LineRenderer>();
  }

  void Update() {
    attack = attackKnob.value;
    decay = decayKnob.value;
    sustain = sustainKnob.value;
    release = releaseKnob.value;

    Vector3 startPos = new Vector3(-.5f, -.5f, -.01f);
    line.widthMultiplier = lineWidth;
    line.positionCount = 5;
    line.SetPositions(new[] {
      startPos,
      startPos + new Vector3(attack * xScale, 1, 0),
      startPos + new Vector3((attack + decay) * xScale, sustain, 0),
      startPos + new Vector3((attack + decay + 1) * xScale, sustain, 0),
      startPos + new Vector3((attack + decay + 1 + release) * xScale, 0, 0),
    });
  }

  public float? GetVolume(
    double pressSample, double? releaseSample, double sample, double sampleFrequency
  ) {
    if (releaseSample == null) {
      float pressTime = (float)((sample - pressSample) / sampleFrequency);

      if (pressTime < attack) {
        return Mathf.Lerp(0, 1, pressTime / attack);
      }
      if (pressTime < attack + decay) {
        return Mathf.Lerp(1, sustain, (pressTime - attack) / decay);
      }
      return sustain;
    }
    else {
      float releaseTime = (float)((sample - releaseSample) / sampleFrequency);

      if (releaseTime < release) {
        return Mathf.Lerp(sustain, 0, releaseTime / release);
      }
      return null;
    }
  }
}
