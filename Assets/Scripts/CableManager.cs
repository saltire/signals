using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

class Cable {
  public InputPort input;
  public OutputPort output;
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

  CameraUtil cameraUtil;

  Cable heldCable;
  List<Cable> cables = new List<Cable>();

  void Awake() {
    cameraUtil = FindObjectOfType<CameraUtil>();

    InitCables();
  }

  public void InitCables() {
    ClearCables();

    // Create cable lines for all existing input-output connections.
    foreach (InputPort input in FindObjectsOfType<InputPort>()) {
      if (input.IsConnected()) {
        LineRenderer line = NewLine();
        line.SetPositions(new[] {
          input.transform.position,
          input.connectedOutput.transform.position,
        });

        input.connectedOutput.connectedInput = input;
        cables.Add(new Cable() { input = input, output = input.connectedOutput, line = line });
      }
    }
    foreach (OutputPort output in FindObjectsOfType<OutputPort>()) {
      if (output.IsConnected() && !cables.Exists(c => c.output == output)) {
        LineRenderer line = NewLine();
        line.SetPositions(new[] {
          output.connectedInput.transform.position,
          output.transform.position,
        });

        output.connectedInput.connectedOutput = output;
        cables.Add(new Cable() { input = output.connectedInput, output = output, line = line });
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
        Vector3 holdPosition = cameraUtil.MousePositionOnPlane(
          Mouse.current.position.ReadValue(), cableHoldY);

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

  LineRenderer NewLine() {
    LineRenderer line = Instantiate(cablePrefab);
    line.transform.parent = transform;

    // line.widthMultiplier = cableColliderWidth;
    // Mesh mesh = new Mesh();
    // line.BakeMesh(mesh, false);
    // line.GetComponent<MeshCollider>().sharedMesh = mesh;
    line.widthMultiplier = cableWidth;

    return line;
  }

  public PortSide? GetHoldingSide() {
    if (heldCable == null) {
      return null;
    }
    return heldCable.input == null ? PortSide.Input : PortSide.Output;
  }

  public PortType? GetHoldingType() {
    if (heldCable == null) {
      return null;
    }
    return heldCable.input != null ? heldCable.input.Type : heldCable.output.Type;
  }

  public void OnPortEnter(Port port) {
    Cable connectedCable = cables
      .Find(c => port is InputPort ? port == c.input : port == c.output);
    bool portHasCable = connectedCable != null;
    bool holdingCable = heldCable != null;
    PortSide? holdingSide = GetHoldingSide();
    PortType? holdingType = GetHoldingType();

    // Turn port green if clicking will connect a cable.
    if (!portHasCable && (!holdingCable ||
      (holdingType == port.Type && holdingSide == port.Side))) {
      port.SetColor(Color.green);
    }
    // Turn port yellow if clicking will disconnect a cable.
    else if (portHasCable && (!holdingCable || heldCable == connectedCable)) {
      port.SetColor(Color.yellow);
    }
  }

  public void OnPortClick(Port port) {
    Cable connectedCable = cables
      .Find(c => port is InputPort ? port == c.input : port == c.output);
    bool portHasCable = connectedCable != null;
    bool holdingCable = heldCable != null;
    PortSide? holdingSide = GetHoldingSide();
    PortType? holdingType = GetHoldingType();

    if (!holdingCable) {
      if (!portHasCable) {
        // Create a cable connected to the port and hold it.
        LineRenderer line = NewLine();

        heldCable = new Cable() {
          input = port is InputPort ? (InputPort)port : null,
          output = port is InputPort ? null : (OutputPort)port,
          line = line,
        };
        cables.Add(heldCable);

        OnPortEnter(port);
      }
      else {
        // Disconnect the cable from the port and hold it.
        connectedCable.input.connectedOutput = null;
        connectedCable.output.connectedInput = null;

        if (port is InputPort) {
          connectedCable.input = null;
        }
        else {
          connectedCable.output = null;
        }

        heldCable = connectedCable;

        OnPortEnter(port);
      }
    }
    else if (!portHasCable && holdingSide == port.Side && holdingType == port.Type) {
      // Connect the held cable to the port and stop holding it.
      if (port is InputPort) {
        heldCable.input = (InputPort)port;
      }
      else {
        heldCable.output = (OutputPort)port;
      }

      heldCable.input.connectedOutput = heldCable.output;
      heldCable.output.connectedInput = heldCable.input;
      heldCable.line.SetPositions(new[] {
        heldCable.input.transform.position,
        heldCable.output.transform.position,
      });

      heldCable = null;

      OnPortEnter(port);
    }
    else if (port == heldCable.input || port == heldCable.output) {
      // Destroy the held cable.
      cables.Remove(heldCable);
      Destroy(heldCable.line.gameObject);
      heldCable = null;

      OnPortEnter(port);
    }
  }
}
