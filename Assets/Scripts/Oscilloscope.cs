using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Oscilloscope : MonoBehaviour, ISignalNode {
  SignalInput input;
  LineRenderer line;

  public Vector2 scale = Vector2.one;
  double[] values = new double[] { 0, 0 };

  void Start() {
    input = GetComponentInChildren<SignalInput>();
    line = GetComponentInChildren<LineRenderer>();

    line.widthMultiplier = .01f;
  }

  void Update() {
    Vector3 startPos = line.transform.position + line.transform.rotation * Vector3.left * scale.x / 2;

    float unitWidth = scale.x / Mathf.Max(1, values.Length - 1);

    line.positionCount = values.Length;
    line.SetPositions(values
      .Select((v, i) => startPos + line.transform.rotation * new Vector3(i * unitWidth, (float)v * scale.y, -.1f))
      .ToArray());
  }

  public double GetValue(double sample, Stack<ISignalNode> nodes) {
    return input.GetValue(sample, nodes);
  }

  public double[] GetValues(double sample, int count, Stack<ISignalNode> nodes) {
    values = input.GetValues(sample, count, nodes);
    return values;
  }
}
