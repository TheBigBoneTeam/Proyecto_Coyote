using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAssetBehaviourRunner : AssetBehaviourRunner
{
    public PushPerception PlayerAttackPerception { get; private set; }
    public PushPerception PlayerHitDefensePerception { get; private set; }
    public PushPerception EndAttackQueue { get; private set; }
  protected  Enemy enemy;
    protected Player player;


    protected override void ModifyGraphs(Dictionary<string, BehaviourGraph> graphMap, Dictionary<string, PushPerception> pushPerceptionMap)
    {
        PlayerAttackPerception = pushPerceptionMap["PlayerAttackPerception"];
        PlayerHitDefensePerception = pushPerceptionMap["PlayerHitDefensePerception"];
        EndAttackQueue = pushPerceptionMap["EndQueue"];

    }
    public void FirePlayerAttack()
    {
        print("FirePlayerAttack");
        PlayerAttackPerception.Fire();
    }
    public void FirePlayerHitDefense()
    {
        print("PlayerHitDefense");
        PlayerHitDefensePerception.Fire();

    }
    public void endQueue()
    {
        print("endQueue" + name);
        EndAttackQueue.Fire(Status.Success);

    }
    public virtual void restart()
    {
        if (player == null)
        {
            enemy = GetComponent<Enemy>();
            player = FindAnyObjectByType<Player>();
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        print("EnableCharacter");
    }

}

