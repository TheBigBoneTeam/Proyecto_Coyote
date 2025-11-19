using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEngine;
using UnityEngine.AI;

public class RunForCoverAction : UnityAction
{
    NavMeshAgent agent;
    Cover coverObj;
    EnemyAI enemyAI;
        bool stopped;
    public override Status Update()
    {

        if (stopped)
        {
            return Status.None;
        }
        if (agent.pathPending)
        {
            return Status.Running;
        }
        Debug.Log($"{agent.pathPending} {agent.remainingDistance} {agent.stoppingDistance} {agent.hasPath} {agent.velocity}");
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
        if (stopped)
        {
            return Status.None;
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
        stopped = true;
        Debug.Log("endrunaction");
        agent.ResetPath();
        base.Stop();
    }
    public override void Start()
    {
        Debug.Log("StartRunCover");
        stopped = false;
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        agent = context.GameObject.GetComponent<NavMeshAgent>();
        coverObj = context.GameObject.GetComponent<Enemy>().CombatArea.getCoverSpot(enemyAI.GetComponent<Enemy>(), out Vector3 hidePosition, out int coverIndex);
        if (coverObj != null)
        {
            context.GameObject.GetComponent<DistanceEnemyAssetBehaviourRunner>().setCover(coverObj, coverIndex);
            UnityEngine.Debug.Log(hidePosition);
            Debug.Log(agent.SetDestination(hidePosition));
            enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);
        }
        else
        {
            Debug.Log("coverNotFound");
        }
    }

}
