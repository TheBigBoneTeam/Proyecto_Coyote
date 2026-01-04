using BehaviourAPI.Core;
using BehaviourAPI.UnityToolkit;
using BehaviourAPI.UnityToolkit.GUIDesigner.Runtime;
using Services;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAssetBehaviourRunner : AssetBehaviourRunner
{
  protected  IGameStateManager gameStateManager;
    public PushPerception PlayerAttackPerception { get; private set; }
    public PushPerception PlayerHitDefensePerception { get; private set; }
    public PushPerception EndAttackQueue { get; private set; }

    public PushPerception BombChargingPerception { get; private set; }
    public PushPerception BombStopChargingPerception { get; private set; }

    public BombEnemyAssetBehaviourRunner _currenteBomb;


    protected Enemy enemy;
    protected Player player;


    protected override void ModifyGraphs(Dictionary<string, BehaviourGraph> graphMap, Dictionary<string, PushPerception> pushPerceptionMap)
    {
        PlayerAttackPerception = pushPerceptionMap["PlayerAttackPerception"];
        PlayerHitDefensePerception = pushPerceptionMap["PlayerHitDefensePerception"];
        EndAttackQueue = pushPerceptionMap["EndQueue"];
        BombChargingPerception = pushPerceptionMap["BombChargingPerception"];
        BombStopChargingPerception = pushPerceptionMap["BombStopChargingPerception"];
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
    public void BombCharging(BombEnemyAssetBehaviourRunner bomb)
    {
        print("BombCharging");
        bomb.subscribeToCharge(bombcharge);
        _currenteBomb = bomb;
        BombChargingPerception.Fire();

    }

    private void bombcharge(BombEnemyAssetBehaviourRunner arg0, bool isCharging)
    {
        print("chargeeee"+name);
        if (!isCharging)
        {
            _currenteBomb = null;
            BombStopChargingPerception.Fire();
        }
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
            gameStateManager = ServiceLocator.Instance.Get<IGameStateManager>();
            enemy = GetComponent<Enemy>();
            player = FindAnyObjectByType<Player>();
        }
    }
    protected override void Init()
    {
        
        base.Init();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        
        print("EnableCharacter");
    }

}

