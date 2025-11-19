using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEngine;
using UnityEngine.AI;

public class RunToHeavyAction : UnityAction
{
    NavMeshAgent agent;
    EnemyAI enemyAI;
    bool stopped;
    BombEnemyAssetBehaviourRunner enemyRunner;
    float currentDist;
    bool isReachable;
    Enemy enemy;
    baseBullet bullet;
    public override Status Update()
    {

        if (stopped)
        {
            return Status.None;
        }
        if (!isReachable || enemyRunner.currentHeavy == null)
        {
            return Status.Failure;
        }
        if (agent.pathPending)
        {
            return Status.Running;
        }
        if (Time.frameCount % 5 == 0)
        {
            agent.SetDestination(enemyRunner.currentHeavy.transform.position);
        }
        
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                enemyAI.LoadBasicAction(EnemyAI.BasicActions.CombatIdle, true);
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
        
        Debug.Log("StartRunHeavy");
        stopped = false;
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        enemy = enemyAI.gameObject.GetComponent<Enemy>();
        agent = enemyAI.GetComponent<NavMeshAgent>();
        bullet = enemyAI.GetComponent<baseBullet>();
        enemyRunner = enemyAI.GetComponent<BombEnemyAssetBehaviourRunner>();
        currentDist = int.MaxValue;
        agent.updateRotation = true;
        foreach (baseBullet bul in enemy.CombatArea.getAllBullets())
       
        if (enemyRunner.currentHeavy == null)
        {
            isReachable = false;
            return;
        }
        if (agent.SetDestination(enemyRunner.currentHeavy.transform.position))
        {
            isReachable = true;
            enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);
        }
        else
        {
            isReachable = false;
        }

    }

}