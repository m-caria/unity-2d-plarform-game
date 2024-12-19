using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FireTrapBlock))]
public class FireTrapBlockEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FireTrapBlock fireTrapBlock = (FireTrapBlock)target;
        if (GUILayout.Button("Generate Traps"))
            fireTrapBlock.Generate();
    }
}
