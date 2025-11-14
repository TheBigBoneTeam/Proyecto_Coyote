using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using Services;
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

        if (stopped)
        {
            return Status.None;
        }

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
        coverObj = context.GameObject.GetComponent<Enemy>().CombatArea.getCoverSpot(enemyAI.GetComponent<Enemy>(), out Vector3 hidePosition, out int coverIndex);
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
        agent.updateRotation = true;

    }

}

public class WalkToPlayerActionCircle : UnityAction
{
    NavMeshAgent agent;
    Player player;
    EnemyAI enemyAI;
    bool stopped;
    Transform CirclePoint;
    bool correctingPos;
    public override Status Update()
    {
        if (stopped)
        {
            return Status.None;
        }
        if (Time.frameCount % 5 == 0)
        {
            Vector3 lookTarget = new Vector3(player.transform.position.x, context.Transform.position.y, player.transform.position.z);
            context.Transform.LookAt(lookTarget);

            agent.SetDestination(CirclePoint.transform.position);
        }
        //Debug.Log(player == null);
        //Debug.Log(player.transform.position);
        //Debug.Log(player.name);

        //Debug.Log(agent.remainingDistance);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                if (!correctingPos)
                {
                    enemyAI.LoadBasicAction(EnemyAI.BasicActions.CombatIdle, true);
                }
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
        CirclePoint = ServiceLocator.Instance.Get<IEnemyManager>().getPoint(enemyAI.KungFuCirclePoint, enemyAI.GetComponent<Enemy>());
        //CirclePoint = ServiceLocator.Instance.Get<IEnemyManager>().getClosestPoint(enemyAI.GetComponent<Enemy>(),out int index);
        if (CirclePoint != null)
        {
            agent.SetDestination(CirclePoint.position);
            agent.updateRotation = false;
            Debug.Log("Remaining Distance" + Vector3.Distance(agent.transform.position, CirclePoint.transform.position));
            if (Vector3.Distance(agent.transform.position, CirclePoint.transform.position) > (agent.stoppingDistance + 1f))
            {
                correctingPos = true;
                enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);
            }
        }
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
    bool thrown;
    bool isBomb;
    Enemy enemy;
    public override Status Update()
    {

        //if (stopped)
        //{
        //    return Status.None;
        //}
        if (!isReachable || enemyRunner.currentAmmo == null)
        {
            return Status.Failure;
        }
        if (isBomb)
        {
            if (Time.frameCount % 5 == 0)
            {
                agent.SetDestination(enemyRunner.currentAmmo.transform.position);
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
        baseBullet bestAmmo = null;
        foreach (baseBullet bul in enemy.CombatArea.getAllBullets())
        {
            if(bul.owner != null && bul.owner != enemy)
            {
                continue;
            }
            float newDist = Vector3.Distance(bul.transform.position,enemyAI.transform.position);
            if (bestAmmo == null || currentDist > newDist)
            {
                currentDist = newDist;
                bestAmmo = bul;
                
            }
        }
        if (bestAmmo == null)
        {
            enemyRunner.hasAmmo = false;
            isReachable = false;
            return;
        }
        if (agent.SetDestination(bestAmmo.transform.position))
        {
            agent.updateRotation = true;
            isReachable = true;
            enemyRunner.hasAmmo = true;
            enemyRunner.currentAmmo = bestAmmo;
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
        }

    }

}
public class CirclePlayerAction : UnityAction
{
    NavMeshAgent agent;
    EnemyAI enemyAI;
    bool stopped;
    float currentDist;
    bool finished;
    Player player;
    public override void Start()
    {
        stopped = false;
        finished = false;
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        agent = context.GameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindAnyObjectByType<Player>();
        agent.SetDestination(player.transform.position);
        
        enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);
        Vector3 vec = Random.insideUnitCircle.normalized;
        vec.z = vec.y;
        vec.z = 0;
        agent.SetDestination(player.transform.position + enemyAI.attackDistance * vec);
        enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);
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
    public override Status Update()
    {
        if (finished)
        {
            return Status.Success;
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                enemyAI.LoadBasicAction(EnemyAI.BasicActions.CombatIdle, true);
                finished = true;
                agent.ResetPath(); return Status.Success;


            }
        }
        return Status.Running;

    }
}
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