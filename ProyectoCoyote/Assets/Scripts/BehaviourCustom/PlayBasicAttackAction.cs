using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class PlayBasicAttackAction : UnityAction
{
    public EnemyAI.BasicActions action;
    public bool idle;
    EnemyAI enemyAI;
    public override Status Update()
    {
        if (idle)
        {
            enemyAI.endActionNode();

            return Status.Success;
        }
        if (!idle && enemyAI.endAction)
        {
            Debug.Log("success");
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
        enemyAI.LoadBasicAction(action,idle);
    }
}
public class RunForCoverAction : UnityAction
{
    NavMeshAgent agent;
    Cover coverObj;
    EnemyAI enemyAI;
        bool stopped;

    public override Status Update()
    {
        Debug.Log("updaterunaction");

        if (stopped)
        {
            return Status.None;
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                enemyAI.LoadBasicAction(EnemyAI.BasicActions.CombatIdle, true);
                context.GameObject.GetComponent<DistanceEnemyAssetBehaviourRunner>().reachCover();
                 agent.ResetPath();

                return Status.Success;
            }
        }
        return Status.Running;

    }
    public override void Pause()
    {
        base.Pause();
        agent.ResetPath();
    }
    public override void Stop()
    {
        base.Stop();
        stopped = true;
        Debug.Log("endrunaction");
        agent.ResetPath();

    }
    public override void Start()
    {
        stopped = false;
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        agent = context.GameObject.GetComponent<NavMeshAgent>();
       coverObj = GameObject.FindAnyObjectByType<combatAreaManager>().getCoverSpot(out Vector3 hidePosition);
        if (coverObj != null)
        {
            UnityEngine.Debug.Log(hidePosition);
            agent.SetDestination(hidePosition);
            enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);
        }
    }

}
public class RandomActionAction : UnityAction
{
    public string BaseAction;
    public string FirstLetter;
    public string LastLetter;
    public bool idle;
    EnemyAI enemyAI;

    public override Status Update()
    {
        if (idle)
        {
            enemyAI.endActionNode();

            return Status.Success;
        }
        if (!idle && enemyAI.endAction)
        {
            Debug.Log("success");
            enemyAI.endActionNode();
            return Status.Success;
        }
        return Status.Running;
    }
    public override void Start()
    {
        char firstletter = FirstLetter[0];
        char lastletter = LastLetter[0];
        int nums = lastletter - firstletter;
        char letter = (char)('A' + Random.Range(0, nums));
        Debug.Log(nums + ":"+letter);

        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        if (enemyAI.endAction)
        {
            Debug.Log("cagada");
        }
        enemyAI.LoadAction(BaseAction +letter, idle);
    }

}
