using CombatEffect;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "AttackData", menuName = "ScriptableObjects/AttackData", order = 1)]

public class AttackData : ScriptableObject
{
    [field: SerializeField] public AnimationClip clip { get; private set; }
    [field: SerializeField] public HittableTypes HitCheckType { get; private set; }

    [field: SerializeField] public HitDirections[] HitDirections { get; private set; }

    [SerializeField][SerializeReference] public List<ACombatEffect> effects;
    [ContextMenu("Add Stun")]
    public void AddStun()
    {
        effects.Add(new StunEffect(null, 10));
    }
    [ContextMenu("Add Damage")]
    public void AddDamage()
    {
        effects.Add(new DamageEffect(null, 10));
    }

}
