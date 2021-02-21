using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Oscilloscope : MonoBehaviour, ISignalNode {
  SignalInput input;
  LineRenderer line;

  Queue<double> queue = new Queue<double>();

  [Range(128, 2048)]
  public int windowSize = 1024;
  [Range(0, 1024)]
  public int phaseShift = 0;

  void Start() {
    input = GetComponentInChildren<SignalInput>();
    line = GetComponentInChildren<LineRenderer>();

    line.widthMultiplier = .01f;
  }

  void Update() {
    int length = phaseShift + windowSize;
    while (queue.Count >= length) {
      Vector2 scale = line.transform.localScale;
      Vector3[] positions = new Vector3[windowSize];
      Vector3 startPos = line.transform.position +
        line.transform.rotation * Vector3.left * scale.x / 2;
      float unitWidth = scale.x / Mathf.Max(1, windowSize - 1);

      for (int i = 0; i < length; i++) {
        double value = queue.Dequeue();
        if (i >= phaseShift) {
          int p = i - phaseShift;
          positions[p] = startPos +
            line.transform.rotation *
            new Vector3(p * unitWidth, Mathf.Clamp((float)value, -1, 1) * scale.y / 2, -.1f);
        }
      }
      line.positionCount = positions.Length;
      line.SetPositions(positions);
    }
  }

  public double GetValue(double sample, Stack<ISignalNode> nodes) {
    return input.GetValue(sample, nodes);
  }

  public double[] GetValues(double sample, int count, Stack<ISignalNode> nodes) {
    double[] values = input.GetValues(sample, count, nodes);
    foreach (double value in values) {
      queue.Enqueue(value);
    }
    return values;
  }
}
