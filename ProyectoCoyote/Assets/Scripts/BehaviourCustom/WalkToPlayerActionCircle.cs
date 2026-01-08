using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using Services;
using UnityEngine;
using UnityEngine.AI;

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
        if (agent.pathPending)
        {
            return Status.Running;
        }
        if (Time.frameCount % 5 == 0)
        {
            Vector3 lookTarget = new Vector3(player.transform.position.x, context.Transform.position.y, player.transform.position.z);
            context.Transform.LookAt(lookTarget);

            agent.SetDestination(CirclePoint.transform.position);
        }
        if(Time.frameCount % 60==0)
        {
            int index = -1;
         Transform   CirclePointTest = ServiceLocator.Instance.Get<IEnemyManager>().getClosestPoint(enemyAI.GetComponent<Enemy>(), out index);
            if(CirclePointTest != null && CirclePointTest != CirclePoint)
            {
                enemyAI.ReturnKungFuPoint();
                enemyAI.KungFuCirclePoint = index;
                CirclePoint = CirclePointTest;
            }
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
        if (correctingPos && Vector3.Distance(agent.transform.position, CirclePoint.transform.position) > (agent.stoppingDistance + 1f))
        {
            correctingPos = false;
            enemyAI.LoadBasicAction(EnemyAI.BasicActions.Walk, true);
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
        int index = enemyAI.KungFuCirclePoint;
        CirclePoint = null;
        if(enemyAI.KungFuCirclePoint >= 0)
        {
            CirclePoint = ServiceLocator.Instance.Get<IEnemyManager>().getPoint(enemyAI.KungFuCirclePoint, enemyAI.GetComponent<Enemy>());
        }
        if (CirclePoint == null)
        {
            CirclePoint = ServiceLocator.Instance.Get<IEnemyManager>().getClosestPoint(enemyAI.GetComponent<Enemy>(), out index);
        }
      //  CirclePoint = ServiceLocator.Instance.Get<IEnemyManager>().getPoint(enemyAI.KungFuCirclePoint, enemyAI.GetComponent<Enemy>());
        //CirclePoint = ServiceLocator.Instance.Get<IEnemyManager>().getClosestPoint(enemyAI.GetComponent<Enemy>(),out int index);
        if (CirclePoint != null)
        {
            enemyAI.KungFuCirclePoint = index;
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
