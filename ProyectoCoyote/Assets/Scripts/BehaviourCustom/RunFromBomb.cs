using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

public class RunFromBomb : UnityAction
{
    private EnemyAI enemyAI;
    public float runDistance;
    private bool destinationFound;
    NavMeshAgent agent;
    bool stopped;


    public override void Stop()
    {
        base.Stop(); 
        agent.ResetPath();
    }
    public override Status Update()
    {

        if (agent.pathPending)
        {
            return Status.Running;
        }
      
        if (!destinationFound)
            return Status.Failure;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
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
    public override void Start()
    {
        stopped = false;
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        agent = context.GameObject.GetComponent<NavMeshAgent>();
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        Debug.Log("RUNFROMBOMB"+context.Transform.name);

        EnemyAssetBehaviourRunner enemyAsset = context.GameObject.GetComponent<EnemyAssetBehaviourRunner>();
  
        Vector3 bombPos = enemyAsset._currenteBomb.transform.position;
        Vector3 origin = context.Transform.position;

        bombPos.y = origin.y;

        Vector3 baseDir = (origin - bombPos).normalized;

        NavMeshHit hit;
        NavMeshPath path = new NavMeshPath();

        for (int i = 0; i < 12; i++)
        {
            float angle = (i == 0) ? 0f : (i % 2 == 0 ? 1 : -1) * ((i + 1) / 2) * 30f;
            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * baseDir;
            Vector3 testPos = origin + dir * runDistance;
            if (NavMesh.SamplePosition(testPos, out hit, 4f, NavMesh.AllAreas))
            {
                if (agent.CalculatePath(hit.position, path) &&
                    path.status == NavMeshPathStatus.PathComplete)
                {
                    agent.SetDestination(hit.position);
                    enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);
                    destinationFound = true;
                    break;
                }
            }
        }
    }

}
