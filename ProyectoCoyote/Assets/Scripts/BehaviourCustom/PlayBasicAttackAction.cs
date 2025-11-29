using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using Services;
using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class PlayBasicAttackAction : UnityAction
{
    public EnemyAI.BasicActions action;
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
        bool isBlock = false;
        if (action.ToString().Contains("Stance"))
        {
            isBlock = true;
            if (enemyAI.loopBlockingAction)
            {
                enemyAI.modifyActionLoop(loops);
                return;
            }
        }
        enemyAI.LoadBasicAction(action,idle,loops,isBlock);
    }
}
