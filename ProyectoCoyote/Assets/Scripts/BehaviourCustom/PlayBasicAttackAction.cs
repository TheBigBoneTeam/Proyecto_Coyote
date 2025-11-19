using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using Services;
using System;
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
    public override void Start()
    {
        base.Start();
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
            agent.updateRotation = true;
            isReachable = true;
        }
    }
}

public class AIWanderer : MonoBehaviour
{
    [SerializeField] Transform[] wanderPoints;
    bool random;
   [SerializeField] int currentPoint;

    private void Start()
    {
        currentPoint = -1;
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeToRestart(restart);
    }

    private void restart()
    {
        currentPoint =-1;
    }

    public Transform getRandomPoint(out int next, int current = -1)
    {
        if (wanderPoints == null || wanderPoints.Length == 0)
        {
            throw new Exception($"AI wanderer of object {name} doesnt have any asigned wanderPoints");
        }
        if (wanderPoints.Length == 1)
        {
            next = 0;
            return wanderPoints[0];
        }
        int returnPoint;
        do
        {
            returnPoint = UnityEngine.Random.Range(0, wanderPoints.Length);
        } while (returnPoint == current);
        next = returnPoint;
        return wanderPoints[returnPoint];

    }
    public Transform getNextPoint(out int next, int current = -1)
    {
        if (current < -1)
        {
            throw new IndexOutOfRangeException("Current cant be less than 0");
        }
        next = (current + 1) % wanderPoints.Length;
        return wanderPoints[next];
    }
    public Transform getPoint(int current = -1)
    {
        if (random)
        {
            return getRandomPoint(out currentPoint, currentPoint);
        }
        else
        {
            return getNextPoint(out currentPoint, currentPoint);
        }
    }
}