using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Reaction))]
public class EReaction : Editor
{
    Reaction reaction;
    private void OnEnable()
    {

        Reaction reaction = (Reaction)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Add Damage"))
        {
            reaction.AddDamage();
        }
        if (GUILayout.Button("Add Stun"))
        {
            reaction.AddStun();
        }
        if (GUILayout.Button("Add Crit Damage"))
        {
            reaction.AddCritDamage();
        }
    }
}