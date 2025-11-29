using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using UnityEngine;

public class PlayAttackAction : UnityAction
{

    /// <summary>
    /// El Scriptable Object del ataque
    /// </summary>
    [SerializeField] string attack;
    public bool idle;
    public int loops = 1;
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
          //  Debug.Log("success");
            enemyAI.endActionNode();
            return Status.Success;
        }
        return Status.Running;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public override void Start()
    {
        Debug.Log(attack);
        enemyAI = context.GameObject.GetComponent<EnemyAI>();
        bool isBlock = false;
        if (attack.Contains("Stance"))
        {
            isBlock = true;
            if (enemyAI.loopBlockingAction)
            {
                enemyAI.modifyActionLoop(loops);
                return;
            }
        }
        enemyAI.LoadAction(attack,idle,loops,isBlock);
    }


}
