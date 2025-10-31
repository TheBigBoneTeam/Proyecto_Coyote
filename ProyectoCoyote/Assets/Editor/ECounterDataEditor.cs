using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ReactionData))]
public class EReactionDataEditor : Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        ReactionData data = (ReactionData)target;
        if (GUILayout.Button("Add Damage"))
        {
            data.AddDamage();
        }
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
