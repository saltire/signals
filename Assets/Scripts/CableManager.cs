using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

class Cable {
  public SignalInput input;
  public SignalOutput output;
  public LineRenderer line;
}

public enum Holding {
  None,
  Input,
  Output,
}

[ExecuteAlways]
public class CableManager : MonoBehaviour {
  public LineRenderer cablePrefab;
  public float cableWidth = .1f;
  public float cableColliderWidth = .3f;
  public float cableHoldY = 3;
  Plane cableHoldPlane;

  Camera mainCamera;

  Cable heldCable;
  List<Cable> cables = new List<Cable>();

  void Awake() {
    mainCamera = FindObjectOfType<Camera>();
    cableHoldPlane = new Plane(Vector3.down, Vector3.up * cableHoldY);

    InitCables();
  }

  public void InitCables() {
    ClearCables();

    // Create cable lines for all existing input-output connections.
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

  public void ClearCables() {
    cables.Clear();
    foreach (LineRenderer line in GetComponentsInChildren<LineRenderer>()) {
      DestroyImmediate(line.gameObject);
    }
  }

  void Update() {
    // Redraw all the cable lines.
    foreach (Cable cable in cables) {
      if (cable == heldCable) {
        Ray mouseRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        float distance;
        cableHoldPlane.Raycast(mouseRay, out distance);
        Vector3 holdPosition = mouseRay.GetPoint(distance);

        heldCable.line.SetPositions(new[] {
          heldCable.input != null ? heldCable.input.transform.position : holdPosition,
          heldCable.output != null ? heldCable.output.transform.position : holdPosition,
        });
      }
      else {
        cable.line.SetPositions(new[] {
          cable.input.transform.position,
          cable.output.transform.position,
        });
      }
    }
  }

  public Holding GetHoldingState() {
    return heldCable == null ? Holding.None :
      (heldCable?.input == null ? Holding.Input : Holding.Output);
  }

  public void OnPortClick(SignalPort port) {
    bool portIsInput = port.GetType() == typeof(SignalInput);
    Cable connectedCable = cables
      .Find(c => portIsInput ? port == c.input : port == c.output);
    bool portHasCable = connectedCable != null;
    Holding holding = GetHoldingState();

    if (holding == Holding.None) {
      if (!portHasCable) {
        // Create a cable connected to the port and hold it.
        LineRenderer line = Instantiate(cablePrefab);
        line.transform.parent = transform;
        line.widthMultiplier = cableWidth;

        heldCable = new Cable() {
          input = portIsInput ? (SignalInput)port : null,
          output = portIsInput ? null : (SignalOutput)port,
          line = line,
        };
      }
      else {
        // Disconnect the cable from the port and hold it.
        connectedCable.input.connectedOutput = null;

        if (portIsInput) {
          connectedCable.input = null;
        }
        else {
          connectedCable.output = null;
        }

        heldCable = connectedCable;
      }
    }
    else if (!portHasCable && holding == (portIsInput ? Holding.Input : Holding.Output)) {
      // Connect the held cable to the port and stop holding it.
      if (portIsInput) {
        heldCable.input = (SignalInput)port;
      }
      else {
        heldCable.output = (SignalOutput)port;
      }

      heldCable.input.connectedOutput = heldCable.output;
      heldCable.line.SetPositions(new[] {
        heldCable.input.transform.position,
        heldCable.output.transform.position,
      });
      cables.Add(heldCable);

      heldCable = null;
    }
    else if ((holding == Holding.Input && port == heldCable.output) ||
      (holding == Holding.Output && port == heldCable.input)) {
      // Destroy the held cable.
      cables.Remove(heldCable);
      Destroy(heldCable.line.gameObject);
      heldCable = null;
    }
  }
}
