using BehaviourAPI.Core;
using System.Collections.Generic;

public class DistanceEnemyAssetBehaviourRunner: EnemyAssetBehaviourRunner{
    public PushPerception ReachCover { get; private set; }

    protected override void ModifyGraphs(Dictionary<string, BehaviourGraph> graphMap, Dictionary<string, PushPerception> pushPerceptionMap)
    {
        base.ModifyGraphs(graphMap, pushPerceptionMap);
        ReachCover = pushPerceptionMap["ReachCover"];
    }

    public void reachCover()
    {
        ReachCover.Fire();  
    }
}