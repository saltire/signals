using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableManager : MonoBehaviour {
  public LineRenderer cablePrefab;
  public float cableWidth = .1f;
  public float cableColliderWidth = .3f;

  List<LineRenderer> cables = new List<LineRenderer>();

  void Start() {
    foreach (SignalInput input in FindObjectsOfType<SignalInput>()) {
      if (input.IsConnected()) {
        LineRenderer cable = Instantiate(cablePrefab);
        cable.transform.parent = transform;
        cable.SetPositions(new[] { input.transform.position, input.connectedOutput.transform.position });

        cable.widthMultiplier = cableColliderWidth;
        Mesh mesh = new Mesh();
        cable.BakeMesh(mesh, false);
        cable.GetComponent<MeshCollider>().sharedMesh = mesh;
        cable.widthMultiplier = cableWidth;
      }
    }
  }
}
