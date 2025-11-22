using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEngine;
using UnityEngine.AI;

public class WalkToPlayerAction : UnityAction
{
    NavMeshAgent agent;
    Player player;
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
        if (Time.frameCount % 5 == 0)
        {
            agent.SetDestination(player.transform.position);
        }
        //Debug.Log(player == null);
        //Debug.Log(player.transform.position);
        //Debug.Log(player.name);

        //Debug.Log(agent.remainingDistance);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                enemyAI.LoadBasicAction(EnemyAI.BasicActions.CombatIdle, true);
                agent.ResetPath();
                Debug.Log("reachPlayer");
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
        Debug.Log("StartRunPlayer");
        stopped = false;
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        agent = context.GameObject.GetComponent<NavMeshAgent>();
            player = GameObject.FindAnyObjectByType<Player>();
            agent.SetDestination(player.transform.position);
            enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);
        agent.updateRotation = true;

    }

}
