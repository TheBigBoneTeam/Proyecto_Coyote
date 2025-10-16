using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;

public class PlayBasicAttackAction : UnityAction
{
    public EnemyAI.BasicActions action;
    public bool idle;
    EnemyAI enemyAI;

    public override Status Update()
    {
        if (!idle && enemyAI.endAction)
        {
            return Status.Success;
        }
        return Status.Running;
    }
    public override void Start()
    {
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        enemyAI.LoadBasicAction(action);
    }
}
