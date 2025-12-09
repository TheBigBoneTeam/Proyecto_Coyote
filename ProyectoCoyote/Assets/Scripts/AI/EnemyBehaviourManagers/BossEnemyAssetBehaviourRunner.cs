using BehaviourAPI.Core;
using System;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemyAssetBehaviourRunner : EnemyAssetBehaviourRunner
{
  [field:SerializeField]  public BossState bossState { get; private set; }
 [SerializeField] GameObject rockTeleportPoint;
    [SerializeField] GameObject groundTeleportPoint;
    NavMeshAgent agent;
    [SerializeField] Transform flyObjective;
    [SerializeField] float flyspeed;

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

       // transform.position = groundTeleportPoint.transform.position;
        agent.enabled = true;

    }
    public override void restart()
    {
        base.restart();
        agent = GetComponent<NavMeshAgent>();
    }
    public void turnOffAgent()
    {
        if (agent != null) {
            agent.enabled = false;
        }
    }
    public void turnOffHookable()
    {
       gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        //  GetComponent<Collider>().enabled = false;
    }
    public void turnOnHookable()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");

        //  GetComponent<Collider>().enabled = true;
    }
    public void turnOnAgent()
    {
        if (agent != null)
        {
            agent.enabled = true;
        }
    }
    public Status fly()
    {
        
        transform.Translate(Vector3.up * Time.deltaTime*flyspeed);

        if(transform.position.y >= flyObjective.position.y)
        {
            return Status.Success;
        }
        return Status.Running;
    }
    public Status fall()
    {
        transform.Translate(Vector3.down * Time.deltaTime * flyspeed);

        if (transform.position.y <= rockTeleportPoint.transform.position.y)
        {
            transform.position = rockTeleportPoint.transform.position;
            return Status.Success;
        }
        return Status.Running;
    }

    public void prepareDown()
    {
        transform.position = new Vector3(rockTeleportPoint.transform.position.x,this.transform.position.y, rockTeleportPoint.transform.position.z);
    }
}
public enum BossState
{
    Melee,
    Distance
}