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
