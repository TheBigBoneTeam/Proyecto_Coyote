using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEngine;
using UnityEngine.AI;

public class CirclePlayerAction : UnityAction
{
    NavMeshAgent agent;
    EnemyAI enemyAI;
    bool stopped;
    float currentDist;
    bool finished;
    Player player;
    public override void Start()
    {
        stopped = false;
        finished = false;
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        agent = context.GameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindAnyObjectByType<Player>();
        agent.SetDestination(player.transform.position);
        
        enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);
        Vector3 vec = UnityEngine.Random.insideUnitCircle.normalized;
        vec.z = vec.y;
        vec.z = 0;
        agent.SetDestination(player.transform.position + enemyAI.attackDistance * vec);
        enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);
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
    public override Status Update()
    {
        if (finished)
        {
            return Status.Success;
        }
        if (agent.pathPending)
        {
            return Status.Running;
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                enemyAI.LoadBasicAction(EnemyAI.BasicActions.CombatIdle, true);
                finished = true;
                agent.ResetPath(); return Status.Success;


            }
        }
        return Status.Running;

    }
}
