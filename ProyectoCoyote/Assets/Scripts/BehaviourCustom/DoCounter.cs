using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEngine;

public class DoCounter : UnityAction
{
    EnemyAI enemyAI;

    public override void Start()
    {
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        enemyAI.startCounter();
    }
    public override Status Update()
    {
        if (enemyAI.endAction)
        {
            Debug.Log("success");
            enemyAI.endActionNode();
            return Status.Success;
        }
        return Status.Running;
    }
}