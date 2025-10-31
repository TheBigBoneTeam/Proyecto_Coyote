using CombatEffect;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReactionData", menuName = "ScriptableObjects/Combat/ReactionData", order = 1)]

public class ReactionData : ScriptableObject
{
    [field: SerializeField] public string animState { get; private set; }

    [SerializeField][SerializeReference] public List<ACombatEffect> effects;
    [ContextMenu("Add Stun")]
    public void AddStun()
    {
        effects.Add(new StunEffect(null, 3));
    }
    [ContextMenu("Add Damage")]
    public void AddDamage()
    {
        effects.Add(new DamageEffect(null, 1));
    }
    [ContextMenu("Add Crit Damage")]
    public void AddCritDamage()
    {
        effects.Add(new CritDamageEffect(null, 3));
    }
}
