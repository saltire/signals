using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

class Cable {
  public SignalInput input;
  public SignalOutput output;
  public LineRenderer line;
}

public class CableManager : MonoBehaviour {
  public LineRenderer cablePrefab;
  public float cableWidth = .1f;
  public float cableColliderWidth = .3f;

  Camera mainCamera;

  Cable heldCable;
  List<Cable> cables = new List<Cable>();

  void Start() {
    mainCamera = FindObjectOfType<Camera>();

    foreach (SignalInput input in FindObjectsOfType<SignalInput>()) {
      if (input.IsConnected()) {
        LineRenderer line = Instantiate(cablePrefab);
        line.transform.parent = transform;
        line.SetPositions(new[] { input.transform.position, input.connectedOutput.transform.position });

        // line.widthMultiplier = cableColliderWidth;
        // Mesh mesh = new Mesh();
        // line.BakeMesh(mesh, false);
        // line.GetComponent<MeshCollider>().sharedMesh = mesh;
        line.widthMultiplier = cableWidth;

        cables.Add(new Cable() { input = input, output = input.connectedOutput, line = line });
      }
    }
  }

  void Update() {
    UpdateCurrentCableLine();
  }

  void UpdateCurrentCableLine() {
    if (heldCable != null) {
      Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
      heldCable.line.SetPositions(new[] {
        heldCable.input != null ? heldCable.input.transform.position : mousePosition,
        heldCable.output != null ? heldCable.output.transform.position : mousePosition,
      });
    }
  }

  public void OnInputClick(SignalInput input) {
    Cable cable = cables.Find(c => c.input == input);
    bool holdingCable = heldCable != null;
    bool heldHasInput = heldCable?.input != null;
    bool inputHasCable = cable != null;

    if (holdingCable && !heldHasInput && !inputHasCable) {
      // Connect the held cable to the input.
      heldCable.input = input;
      heldCable.input.connectedOutput = heldCable.output;
      heldCable.line.SetPositions(new[] { heldCable.input.transform.position, heldCable.output.transform.position });
      cables.Add(heldCable);

      heldCable = null;
    }
    else if (holdingCable && heldHasInput && heldCable.input == input) {
      // Destroy the held cable.
      Destroy(heldCable.line.gameObject);
      heldCable = null;
    }
    else if (!holdingCable && !inputHasCable) {
      // Create a cable connected to the input and hold it.
      LineRenderer line = Instantiate(cablePrefab);
      line.transform.parent = transform;
      line.widthMultiplier = cableWidth;

      heldCable = new Cable() { input = input, line = line };
      UpdateCurrentCableLine();
    }
    else if (!holdingCable && inputHasCable) {
      // Disconnect the cable from the input and hold it.
      cable.input.connectedOutput = null;

      cable.input = null;
      cables.Remove(cable);

      heldCable = cable;
      UpdateCurrentCableLine();
    }
  }

  public void OnOutputClick(SignalOutput output) {
    Cable cable = cables.Find(c => c.output == output);
    bool holdingCable = heldCable != null;
    bool heldHasOutput = heldCable?.output != null;
    bool outputHasCable = cable != null;

    if (holdingCable && !heldHasOutput && !outputHasCable) {
      // Connect the held cable to the output.
      heldCable.output = output;
      heldCable.input.connectedOutput = heldCable.output;
      heldCable.line.SetPositions(new[] { heldCable.input.transform.position, heldCable.output.transform.position });
      cables.Add(heldCable);

      heldCable = null;
    }
    else if (holdingCable && heldHasOutput && heldCable.output == output) {
      // Destroy the held cable.
      Destroy(heldCable.line.gameObject);
      heldCable = null;
    }
    else if (!holdingCable && !outputHasCable) {
      // Create a cable connected to the output and hold it.
      LineRenderer line = Instantiate(cablePrefab);
      line.transform.parent = transform;
      line.widthMultiplier = cableWidth;

      heldCable = new Cable() { line = line, output = output };
      UpdateCurrentCableLine();
    }
    else if (!holdingCable && outputHasCable) {
      // Disconnect the cable from the output and hold it.
      cable.input.connectedOutput = null;

      cable.output = null;
      cables.Remove(cable);

      heldCable = cable;
      UpdateCurrentCableLine();
    }
  }
}
