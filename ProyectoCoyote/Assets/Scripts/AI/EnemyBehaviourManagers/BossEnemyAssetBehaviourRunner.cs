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

  [SerializeField]  int leftDodges,rightDodges;
    [SerializeField] bool learningFromPlayerDodges;

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
        if (player == null)
        {
            FindAnyObjectByType<Player>().subscribeToDodgeAttack(DodgeAttack);
        }
        base.restart();
        agent = GetComponent<NavMeshAgent>();
    }
    public void turnOffAgent()
    {
        if (agent != null) {
            agent.enabled = false;
        }
    }
    protected override void Init()
    {
        base.Init();
    }

    private void DodgeAttack(HitDirections arg0)
    {
        if (!learningFromPlayerDodges)
            return;
        print("playerDodgeAttack");
        if (arg0 == HitDirections.Left)
        {
            leftDodges++;
        }
        if (arg0 == HitDirections.Rigth)
        {
            rightDodges++;
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
    public int ChooseBestDirection()
    {
        int allDodges = leftDodges + rightDodges;
        int random = UnityEngine.Random.Range(0, allDodges);
        if(random < leftDodges)
        {
            return 0;
        }
        return 1;
    }
    public void setLearning(bool learning)
    {
        learningFromPlayerDodges = learning;
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