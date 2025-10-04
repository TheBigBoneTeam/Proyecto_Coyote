using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttackData))]
public class EAttackDataEditor : Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        AttackData data = (AttackData)target;
        if (GUILayout.Button("Add Damage")) {
            data.AddDamage();
        }
        if (GUILayout.Button("Add Stun"))
        {
            data.AddStun();
        }
    }
}
