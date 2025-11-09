using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
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
public class RunForCoverAction : UnityAction
{
    NavMeshAgent agent;
    Cover coverObj;
    EnemyAI enemyAI;
        bool stopped;

    public override Status Update()
    {
        Debug.Log("updaterunaction");

        if (stopped)
        {
            return Status.None;
        }
        Debug.Log(agent.remainingDistance);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                enemyAI.LoadBasicAction(EnemyAI.BasicActions.CombatIdle, true);
                context.GameObject.GetComponent<DistanceEnemyAssetBehaviourRunner>().reachCover();
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
        Debug.Log("StartRunCover");

        stopped = false;
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        agent = context.GameObject.GetComponent<NavMeshAgent>();
       coverObj = context.GameObject.GetComponent<Enemy>().CombatArea.getCoverSpot(out Vector3 hidePosition,out int coverIndex);
        if (coverObj != null)
        {
            context.GameObject.GetComponent<DistanceEnemyAssetBehaviourRunner>().setCover(coverObj, coverIndex);
            UnityEngine.Debug.Log(hidePosition);
            agent.SetDestination(hidePosition);
            enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);
        }
    }

}
public class RandomActionAction : UnityAction
{
    public string BaseAction;
    public string FirstLetter;
    public string LastLetter;
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
        char firstletter = FirstLetter[0];
        char lastletter = LastLetter[0];
        int nums = lastletter - firstletter;
        char letter = (char)('A' + Random.Range(0, nums));
        Debug.Log(nums + ":"+letter);

        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        if (enemyAI.endAction)
        {
            Debug.Log("cagada");
        }
        enemyAI.LoadAction(BaseAction +letter, idle);
    }

}
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
        
    }

}
public class RunToAmmo : UnityAction
{
    NavMeshAgent agent;
    EnemyAI enemyAI;
    bool stopped;
    BullEnemyAssetBehaviourRunner enemyRunner;
    float currentDist;
    bool isReachable;
    public override Status Update()
    {
        Debug.Log("updaterunaction");

        if (stopped)
        {
            return Status.None;
        }
        if (!isReachable)
        {
            return Status.Failure;
        }
        Debug.Log(agent.remainingDistance);

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
        Debug.Log("StartRunCover");

        stopped = false;
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        agent = context.GameObject.GetComponent<NavMeshAgent>();
        enemyRunner = context.GameObject.GetComponent<BullEnemyAssetBehaviourRunner>();
        foreach(baseBullet bul in context.GameObject.GetComponent<Enemy>().CombatArea.getAllBullets())
        {
            float newDist = Vector3.Distance(bul.transform.position,enemyAI.transform.position);
            if (enemyRunner.currentAmmo == null || currentDist > newDist)
            {
                currentDist = newDist;
                enemyRunner.currentAmmo = bul;
            }
        }
        if (enemyRunner.currentAmmo == null)
        {
            enemyRunner.hasAmmo = false;
            return;
        }
        if (agent.SetDestination(enemyRunner.currentAmmo.transform.position))
        {
            isReachable = true;
            enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);

        }
        else
        {
            isReachable = false;
            enemyRunner.hasAmmo = false;
        }

    }

}
