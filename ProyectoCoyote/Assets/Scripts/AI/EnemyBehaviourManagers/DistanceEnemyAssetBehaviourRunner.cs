using BehaviourAPI.Core;
using System.Collections.Generic;
using UnityEngine;

public class DistanceEnemyAssetBehaviourRunner: EnemyAssetBehaviourRunner{
    public PushPerception ReachCover { get; private set; }
    public PushPerception CoverUnsafe { get; private set; }

    LayerMask layer;
  [SerializeField]  bool isUnsafe;


    Cover currentCover;
    int currentCoverHidePos;

    protected override void ModifyGraphs(Dictionary<string, BehaviourGraph> graphMap, Dictionary<string, PushPerception> pushPerceptionMap)
    {
        base.ModifyGraphs(graphMap, pushPerceptionMap);
        ReachCover = pushPerceptionMap["ReachCover"];
        CoverUnsafe = pushPerceptionMap["CoverUnsafe"];
    }

    public void reachCover()
    {
        print("reachCover");
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
    public bool CanShootPlayer()
    {
        if (currentCover == null)
        {
            return false;
        }
        return currentCover.canShootPlayer(player.transform,currentCoverHidePos);
    }
    public bool setCheckUnsafe()
    {
        setUnsafe();
        return checkUnsafe();
    }
    public void setUnsafe()
    {
        if (currentCover == null)
        {
            isUnsafe = true;
        }
        else
        {
            isUnsafe = !currentCover.checkSafe(transform,player.transform, currentCoverHidePos);
        }
        print("unsafe==" +isUnsafe);
    }
    public void setCover(Cover cover, int coverindex)
    {
        if(currentCover != null){
            currentCover.returnOwnerShip(enemy);
        }
        isUnsafe = false;
        currentCover = cover;
        currentCoverHidePos = coverindex;

    }
    public Cover getCurrentCover() => currentCover;
    public void returnCoverOwner()
    {
        if (currentCover != null)
        {
            currentCover.returnOwnerShip(enemy);
            currentCover = null;
        }
    }
    //protected void Start()
    //{
    //    base.start
    //    player = FindAnyObjectByType<Player>();
    //}
}