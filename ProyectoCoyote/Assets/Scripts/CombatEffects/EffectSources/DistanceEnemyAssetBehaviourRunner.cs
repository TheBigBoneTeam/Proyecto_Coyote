using BehaviourAPI.Core;
using System.Collections.Generic;
using UnityEngine;

public class DistanceEnemyAssetBehaviourRunner: EnemyAssetBehaviourRunner{
    public PushPerception ReachCover { get; private set; }
    public PushPerception CoverUnsafe { get; private set; }

    Player player;
    LayerMask layer;
  [SerializeField]  bool isUnsafe;

    Cover currentCover;
    int currentCoverHidePos;

    protected override void ModifyGraphs(Dictionary<string, BehaviourGraph> graphMap, Dictionary<string, PushPerception> pushPerceptionMap)
    {
        player = FindAnyObjectByType<Player>();
        base.ModifyGraphs(graphMap, pushPerceptionMap);
        ReachCover = pushPerceptionMap["ReachCover"];
        CoverUnsafe = pushPerceptionMap["CoverUnsafe"];
    }

    public void reachCover()
    {
        ReachCover.Fire();  
    }
    public void reachUnsafe()
    {

        CoverUnsafe.Fire();
    }
    public bool checkUnsafe()
    {
        return isUnsafe;

    }
    public void setUnsafe()
    {
        isUnsafe = !currentCover.checkSafe(player.transform, currentCoverHidePos);
        print("unsafe==" +isUnsafe);
    }
    public void setCover(Cover cover, int coverindex)
    {
        isUnsafe = false;
        currentCover = cover;
        currentCoverHidePos = coverindex;

    }
    //protected void Start()
    //{
    //    base.start
    //    player = FindAnyObjectByType<Player>();
    //}
}