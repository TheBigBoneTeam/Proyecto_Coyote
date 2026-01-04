using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class RunToAmmo : UnityAction
{
    NavMeshAgent agent;
    EnemyAI enemyAI;
    bool stopped;
    
    BullEnemyAssetBehaviourRunner enemyRunner;
    float currentDist;
    bool isReachable;
    bool thrown;
    bool isBomb;
    Enemy enemy;
    public override Status Update()
    {

        //if (stopped)
        //{
        //    return Status.None;
        //}
        if (!isReachable || enemyRunner.currentAmmo == null || enemyRunner._closestAmmo == null)
        {
            return Status.Failure;
        }
        if (agent.pathPending)
        {
            return Status.Running;
        }
        if (isBomb)
        {
            if (Time.frameCount % 10 == 0)
            {
                agent.SetDestination(enemyRunner.currentAmmo.transform.position);
            }
        }
        else
        {
            if (enemyRunner._closestAmmo != enemyRunner.currentAmmo)
            {
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(enemyRunner._closestAmmo.transform.position, path) &&
                      path.status == NavMeshPathStatus.PathComplete)
                {
                    agent.SetDestination(enemyRunner._closestAmmo.transform.position);
                    enemyRunner.currentAmmo.setOwner(null);
                    enemyRunner.currentAmmo = enemyRunner._closestAmmo;
                    enemyRunner.currentAmmo.setOwner(enemy);
                    enemy.CombatArea.changeInAmmoOwnership();
                }
            }
        }
           
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                enemyAI.LoadBasicAction(EnemyAI.BasicActions.CombatIdle, true);
                agent.ResetPath();
                thrown = true;
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
        if (!thrown && enemyRunner.currentAmmo)
        {
            enemyRunner.currentAmmo.setOwner(null);
            enemy.CombatArea.changeInAmmoOwnership();
        }
        base.Stop();
    }
    public override void Start()
    {
        Debug.Log("StartRunAmmo");
        thrown = false;
        stopped = false;
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        enemy = enemyAI.gameObject.GetComponent<Enemy>();
        agent = enemyAI.GetComponent<NavMeshAgent>();
        enemyRunner = enemyAI.GetComponent<BullEnemyAssetBehaviourRunner>();
        enemyRunner.currentAmmo = null;
        currentDist = int.MaxValue;
        if (enemyRunner._closestAmmo == null)
        {
            enemyRunner.findClosestRock();
        }
        if (enemyRunner._closestAmmo == null)
        {
            enemyRunner.hasAmmo = false;
            isReachable = false;
            return;
        }
            //baseBullet bestAmmo = null;
            //baseBullet[] allBullets = enemy.CombatArea.getAllBullets();
            //if (allBullets != null)
            //{
            //    foreach (baseBullet bul in allBullets)
            //    {
            //        if (bul.owner != null && bul.owner != enemy)
            //        {
            //            continue;
            //        }
            //        float newDist = Vector3.Distance(bul.transform.position, enemyAI.transform.position);
            //        if (bestAmmo == null || currentDist > newDist)
            //        {
            //            currentDist = newDist;
            //            bestAmmo = bul;

            //        }
            //    }
            //}
            //if (bestAmmo == null)
            //{
            //    enemyRunner.hasAmmo = false;
            //    isReachable = false;
            //    return;
            //}
            if (agent.SetDestination(enemyRunner._closestAmmo.transform.position))
        {
            agent.updateRotation = true;
            isReachable = true;
            enemyRunner.hasAmmo = true;
            enemyRunner.currentAmmo = enemyRunner._closestAmmo;
            enemyRunner.currentAmmo.setOwner(enemy);

            enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);
            if (enemyRunner.currentAmmo.GetComponent<BombEnemy>() != null)
            {
                isBomb = true;
            }
        }
        else
        {
            isReachable = false;
            enemyRunner.hasAmmo = false;
            enemyRunner.currentAmmo = null;
        }

    }

}
