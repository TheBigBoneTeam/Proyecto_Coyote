using System;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemyAssetBehaviourRunner : EnemyAssetBehaviourRunner
{
  [field:SerializeField]  public BossState bossState { get; private set; }
 [SerializeField] GameObject rockTeleportPoint;
    [SerializeField] GameObject groundTeleportPoint;
    NavMeshAgent agent;

    public bool checkBossStateDistance()
    {
        return bossState.Equals(BossState.Distance);
    }
    public bool checkBossStateMelee()
    {
        return bossState.Equals(BossState.Melee);
    }
    public void setBossState(BossState bossState)
    {
        this.bossState = bossState;
    }
    public void GoToRock()
    {
        agent.enabled = false;
        transform.position = rockTeleportPoint.transform.position;
        agent.enabled = true;

    }
    public void GoToGround()
    {
        agent.enabled = false;

        transform.position = groundTeleportPoint.transform.position;
        agent.enabled = true;

    }
    public override void restart()
    {
        base.restart();
        agent = GetComponent<NavMeshAgent>();
    }
}
public enum BossState
{
    Melee,
    Distance
}