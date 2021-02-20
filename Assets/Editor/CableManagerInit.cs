using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CableManager))]
public class CableManagerInit : Editor {
  public override void OnInspectorGUI() {
    DrawDefaultInspector();

    CableManager cables = (CableManager)target;

    if (GUILayout.Button("Place Cables")) {
      cables.InitCables();
    }
  }
}
