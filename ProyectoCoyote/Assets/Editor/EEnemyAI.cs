using UnityEditor;

[CustomEditor(typeof(EnemyAI))]
public class EEnemyAI : Editor
{
    SerializedProperty ActionList;

    SerializedProperty endAction;
    SerializedProperty Locked;

    //Se van guardado los ataques para que no se pierdan al borrar algo sin querer
    private AttackData[] backUpStats;

    private void OnEnable()
    {

        endAction = serializedObject.FindProperty("endAction");
        Locked = serializedObject.FindProperty("Locked");

    }
    public override void OnInspectorGUI()
    {
        //Hace falta actualizar los serialez objects
        serializedObject.Update();
        
        EnemyAI enemyAI = (EnemyAI)target;
                //EditorGUILayout.PropertyField(ActionList);
        EditorGUILayout.PropertyField(endAction);
        EditorGUILayout.PropertyField(Locked);

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