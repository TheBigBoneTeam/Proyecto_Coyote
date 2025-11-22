using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using System;
using UnityEngine;
using UnityEngine.AI;

public class WanderAroundAction : UnityAction
{
    EnemyAI enemyAI;
    Transform objective;
    AIWanderer wanderer;
    NavMeshAgent agent;
    bool isReachable;
    public override Status Update()
    {
        if (!isReachable)
        {
            return Status.Failure;
        }
        if (agent.pathPending)
        {
            return Status.Running;
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                enemyAI.LoadBasicAction(EnemyAI.BasicActions.Idle, true);
                agent.ResetPath();
                return Status.Success;
            }
        }
        return Status.Running;
    }
    public override void Stop()
    {
        base.Stop();
        agent.ResetPath();
    }
    public override void Start()
    {
        base.Start();
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        wanderer = context.GameObject.GetComponent<AIWanderer>();
        if (wanderer == null)
        {
            throw new NullReferenceException($"AICharacter {context.GameObject.name} needs component AIWanderer");
        }
        agent = context.GameObject.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            throw new NullReferenceException($"AICharacter {context.GameObject.name} needs component NavMeshAgent");
        }
        objective = wanderer.getPoint();
        if (agent.SetDestination(objective.position))
        {
            enemyAI.LoadAction("Wander", true);
            agent.updateRotation = true;
            isReachable = true;
        }
    }
}
