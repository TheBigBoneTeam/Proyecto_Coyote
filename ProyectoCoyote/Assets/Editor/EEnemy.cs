using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]

public class EEnemy : Editor
{
    Vector3 pos;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Enemy data = (Enemy)target;
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Die"))
            {
                data.Die();
            }
        }
    }
}
[CustomEditor(typeof(BombEnemy))]

public class EBombEnemy : Editor
{
    Vector3 pos;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Enemy data = (Enemy)target;
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Die"))
            {
                data.Die();
            }
        }
    }
}