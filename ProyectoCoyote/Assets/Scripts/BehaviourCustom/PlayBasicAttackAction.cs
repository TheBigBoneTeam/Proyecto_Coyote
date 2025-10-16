using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;

public class PlayBasicAttackAction : UnityAction
{
    public EnemyAI.BasicAttacks attack;
    EnemyAI enemyAI;

    public override Status Update()
    {
        if (enemyAI.endAction)
        {
            return Status.Success;
        }
        return Status.Running;
    }
    public override void Start()
    {
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        enemyAI.LoadBasicAction(attack);
    }
}