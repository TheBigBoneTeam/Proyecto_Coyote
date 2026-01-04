using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEngine;
using UnityEngine.AI;

public class DefendFromBomb : UnityAction
{
    private EnemyAI enemyAI;

    public override Status Update()
    {
        return Status.Running;
    }
    public override void Start()
    {
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        EnemyAssetBehaviourRunner enemyAsset = context.GameObject.GetComponent<EnemyAssetBehaviourRunner>();    
        Vector3 lookTarget = new Vector3(enemyAsset._currenteBomb.transform.position.x, context.Transform.position.y, enemyAsset._currenteBomb.transform.position.z);
        context.GameObject.transform.LookAt(lookTarget);
        if (enemyAI.loopBlockingAction)
        {
            enemyAI.modifyActionLoop(-1);
            return;

        }
        enemyAI.LoadAction("StanceA", false, -1, true);
    }

}
