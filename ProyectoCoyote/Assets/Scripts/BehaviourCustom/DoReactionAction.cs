using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEngine;

public class DoReactionAction : UnityAction
{
    EnemyAI enemyAI;

    public override void Start()
    {
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        enemyAI.startReaction();
    }
    public override Status Update()
    {
        if (enemyAI.endAction)
        {
            Debug.Log("successreaction");
            enemyAI.endReactionCounter();
            return Status.Success;
        }
        return Status.Running;
    }

}
