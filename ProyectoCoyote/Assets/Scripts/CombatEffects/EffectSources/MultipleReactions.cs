using CombatEffect;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MultipleReactions : Reaction
{

   public List<ReactionDataForMultiple> reactionList;
    public override void startReaction()
    {
        int random = Random.Range(0, reactionList.Count);
        ReactionDataForMultiple  reactiondata= reactionList[random];
        // print("reaction" +animName);
        enemyAI.LoadAction(reactiondata.animName);
        effects = reactiondata.effects.effects;
        if (effects != null && effects.Count > 0)
            addEffectsToChar(FindAnyObjectByType<Player>());

    }

}

[System.Serializable]
public class ReactionDataForMultiple
{
    public string animName;
    [SerializeReference] public AttackData effects;

    public void addEffect(int v)
    {
        Debug.Log(v);
    }
}