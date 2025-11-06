using CombatEffect;
using System.Collections.Generic;
using UnityEngine;

public class Reaction:ACombatEffectSource
{
    public string animName;
    EnemyAI enemyAI;


    private void Start()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    public void startReaction()
    {
       // print("reaction" +animName);
        enemyAI.LoadAction(animName);
        if(effects != null && effects.Count > 0) 
        addEffectsToChar(FindAnyObjectByType<Player>());

    }
    public void setAnim(string animName)
    {
        this.animName = animName;
    }

    public void LoadData(ReactionData data)
    {
        print("getData");
        if (data == null)
        {
            effects.Clear();
        }
        else
        {
            animName = data.animState;
            effects.Clear();
            foreach (var effect in data.effects)
            {
                effect.setSource(this);
                effects.Add(effect);
            }
        }
    }
}
