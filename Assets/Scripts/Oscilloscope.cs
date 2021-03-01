using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Oscilloscope : SignalModule {
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
  }

  void Update() {
    int length = phaseShift + windowSize;
    while (queue.Count >= length) {
      Vector3[] positions = new Vector3[windowSize];
      Vector3 startPos = Vector3.left / 2 + Vector3.back * .01f;
      int units = windowSize - 1;

      for (int i = 0; i < length; i++) {
        double value = queue.Dequeue();
        if (i >= phaseShift) {
          int p = i - phaseShift;
          positions[p] = startPos + new Vector3((float)p / units,
            Mathf.Clamp((float)value * yScale, -1, 1) / 2, 0);
        }
      }
      line.widthMultiplier = lineWidth;
      line.positionCount = positions.Length;
      line.SetPositions(positions);
    }
  }

  public override double[] GetValues(double sample, int count, Stack<SignalModule> modules) {
    double[] values = input.GetValues(sample, count, modules);
    foreach (double value in values) {
      queue.Enqueue(value);
    }
    return values;
  }
}
