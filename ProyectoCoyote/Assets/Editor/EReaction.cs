using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Reaction))]
public class EReaction : Editor
{
    Reaction reaction;
    private void OnEnable()
    {
      Reaction  reaction = (Reaction)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Add Damage"))
        {
reaction.a        }
        if (GUILayout.Button("Add Stun"))
        {
            data.AddStun();
        }
        if (GUILayout.Button("Add Crit Damage"))
        {
            data.AddCritDamage();
        }
    }
}