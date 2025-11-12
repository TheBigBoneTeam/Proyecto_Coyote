using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(Gun))]

public class EGun:Editor
{
    Vector3 pos;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Gun data = (Gun)target;
        if (Application.isPlaying)
        {
            EditorGUILayout.Vector3Field("Debug Shoot Pos", pos);
            if (GUILayout.Button("Debug Shoot"))
            {
                data.Shoot(pos);
            }
        }
    }
}
