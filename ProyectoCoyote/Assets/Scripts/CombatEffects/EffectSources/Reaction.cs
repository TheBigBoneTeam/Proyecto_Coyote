using CombatEffect;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Reaction:ACombatEffectSource
{
    public string animName;
    EnemyAI enemyAI;
    [SerializeField][SerializeReference] protected List<ACombatEffect> effects;


    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    public void startReaction()
    {
        enemyAI.LoadAction(animName);
        addEffectsToChar(FindAnyObjectByType<Player>());

    }
}
