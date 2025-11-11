using UnityEditor;

[CustomEditor(typeof(EnemyAI))]
public class EEnemyAI : Editor
{
    SerializedProperty ActionList;

    SerializedProperty endAction;
    SerializedProperty Locked;
    SerializedProperty reactionOn;
    SerializedProperty onAction;

    SerializedProperty counterOn;
    SerializedProperty Reaction;
    SerializedProperty Counter;

    SerializedProperty seeDistance;
    SerializedProperty attackDistance;
    SerializedProperty KungFuCirclePoint;

    //Se van guardado los ataques para que no se pierdan al borrar algo sin querer
    private AttackData[] backUpStats;

    private void OnEnable()
    {

        endAction = serializedObject.FindProperty("endAction");
        Locked = serializedObject.FindProperty("Locked");
        seeDistance = serializedObject.FindProperty("seeDistance");
        attackDistance = serializedObject.FindProperty("attackDistance");
        reactionOn = serializedObject.FindProperty("reactionOn");
        counterOn = serializedObject.FindProperty("counterOn");
        Reaction = serializedObject.FindProperty("reactionObj");
        Counter = serializedObject.FindProperty("counterObj");
        onAction = serializedObject.FindProperty("onAction");
        KungFuCirclePoint = serializedObject.FindProperty("KungFuCirclePoint");
    }
    public override void OnInspectorGUI()
    {
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((EnemyAI)target), GetType(), false);
        //Hace falta actualizar los serialez objects
        serializedObject.Update();
        
        EnemyAI enemyAI = (EnemyAI)target;
                //EditorGUILayout.PropertyField(ActionList);
        EditorGUILayout.PropertyField(endAction);
        EditorGUILayout.PropertyField(Locked);
        EditorGUILayout.PropertyField(seeDistance);
        EditorGUILayout.PropertyField(attackDistance);
        EditorGUILayout.PropertyField(reactionOn);
        EditorGUILayout.PropertyField(counterOn);
        EditorGUILayout.PropertyField(Counter);
        EditorGUILayout.PropertyField(Reaction);
        EditorGUILayout.PropertyField(onAction);
        EditorGUILayout.PropertyField(KungFuCirclePoint);
        //if (enemyAI.ActionList == null || enemyAI.ActionList.Length != System.Enum.GetNames(typeof(EnemyAI.BasicAttacks)).Length)
        //{
        //    backUpStats = enemyAI.ActionList;
        //    enemyAI.ActionList = new AttackData[System.Enum.GetNames(typeof(EnemyAI.BasicAttacks)).Length];
        //    if (backUpStats != null)
        //    {
        //        for (int i = 0; i < backUpStats.Length; i++)
        //        {
        //            if (i < enemyAI.ActionList.Length)
        //            {
        //                enemyAI.ActionList[i] = backUpStats[i];
        //            }
        //        }
        //    }

        //    // Array.Resize(ref player.StatData, Enum.GetNames(typeof(matchEnums.Stats)).Length);

        //}
        //else
        //{
        //    backUpStats = enemyAI.ActionList;

        //}
        //Hace falta actualizar los serialez objects
        serializedObject.ApplyModifiedProperties();

    }
}
