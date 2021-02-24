using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Oscilloscope : SignalNode {
  SignalInput input;
  LineRenderer line;

  Queue<double> queue = new Queue<double>();

  [Range(0, 5)]
  public float yScale = .25f;
  [Range(128, 2048)]
  public int windowSize = 1024;
  [Range(0, 1024)]
  public int phaseShift = 0;

  public float lineWidth = .01f;

  void Start() {
    input = GetComponentInChildren<SignalInput>();
    line = GetComponentInChildren<LineRenderer>();

    line.widthMultiplier = lineWidth;
  }

  void Update() {
    int length = phaseShift + windowSize;
    while (queue.Count >= length) {
      Vector3[] positions = new Vector3[windowSize];
      Vector3 startPos = Vector3.left / 2;
      int units = windowSize - 1;

      for (int i = 0; i < length; i++) {
        double value = queue.Dequeue();
        if (i >= phaseShift) {
          int p = i - phaseShift;
          positions[p] = startPos + new Vector3((float)p / units,
            Mathf.Clamp((float)value * yScale, -1, 1) / 2, -.01f);
        }
      }
      line.positionCount = positions.Length;
      line.SetPositions(positions);
    }
  }

  public override double GetValue(double sample, Stack<SignalNode> nodes) {
    return input.GetValue(sample, nodes);
  }

  public override double[] GetValues(double sample, int count, Stack<SignalNode> nodes) {
    double[] values = input.GetValues(sample, count, nodes);
    foreach (double value in values) {
      queue.Enqueue(value);
    }
    return values;
  }
}
