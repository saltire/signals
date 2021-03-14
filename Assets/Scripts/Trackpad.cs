using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trackpad : SignalModule {
  public Transform handle;
  public SpriteRenderer grid { get; private set; }
  public SignalOutput xOutput;
  public SignalOutput yOutput;

  Vector2 axisValues;

  void Awake() {
    grid = GetComponentInChildren<SpriteRenderer>();
  }

  void Update() {
    Vector2 rawValues = new Vector2(
      Mathf.InverseLerp(grid.bounds.min.x, grid.bounds.max.x, handle.position.x),
      Mathf.InverseLerp(grid.bounds.min.z, grid.bounds.max.z, handle.position.z));

    axisValues = new Vector2(
      Mathf.Lerp(-1, 1, Mathf.InverseLerp(grid.bounds.min.x, grid.bounds.max.x, handle.position.x)),
      Mathf.Lerp(-1, 1, Mathf.InverseLerp(grid.bounds.min.z, grid.bounds.max.z, handle.position.z)));
  }

  public override double[] GetValues(double sample, int count, Stack<SignalModule> modules,
    SignalOutput output) {
    double value = output == xOutput ? axisValues.x : axisValues.y;

    double[] values = new double[count];
    for (int i = 0; i < count; i++) {
      values[i] = value;
    }
    return values;
  }
}
