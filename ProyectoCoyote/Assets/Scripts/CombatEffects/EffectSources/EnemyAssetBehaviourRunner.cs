using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAssetBehaviourRunner : AssetBehaviourRunner
{
    public PushPerception PlayerAttackPerception { get; private set; }
    public PushPerception PlayerHitDefensePerception { get; private set; }
    public PushPerception EndAttackQueue { get; private set; }


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
        PlayerHitDefensePerception.Fire();

    }
    public void endQueue()
    {
        EndAttackQueue.Fire(Status.Success);
    }
}
public class BombEnemyAssetBehaviourRunner : EnemyAssetBehaviourRunner
{
    public PushPerception ChosenAsAmmo { get; private set; }

    protected override void ModifyGraphs(Dictionary<string, BehaviourGraph> graphMap, Dictionary<string, PushPerception> pushPerceptionMap)
    {
    base.ModifyGraphs(graphMap, pushPerceptionMap);
        ChosenAsAmmo = pushPerceptionMap["ChosenAsAmmo"];
    }
}
