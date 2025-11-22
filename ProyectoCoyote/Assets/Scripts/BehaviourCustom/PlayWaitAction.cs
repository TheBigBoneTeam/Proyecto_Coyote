using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEngine;

public class PlayWaitAction : UnityAction
{
    [SerializeField] string attack;
    EnemyAI enemyAI;
    public override Status Update()
    {
        if (enemyAI.hasPriority)
        {
            enemyAI.endActionNode();

            return Status.Success;
        }
        return Status.Running;
    }
    public override void Start()
    {
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        if (enemyAI.endAction)
        {
            Debug.Log("cagada");
        }
        enemyAI.LoadAction(attack);
    }
}
