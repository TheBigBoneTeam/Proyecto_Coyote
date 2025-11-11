using CombatEffect;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "AttackData", menuName = "ScriptableObjects/Combat/AttackData", order = 1)]

public class AttackData : ScriptableObject
{
    [field: SerializeField] public AnimationClip clip { get; private set; }

    [field: SerializeField] public bool isParreable { get; private set; }

    [field: SerializeField] public HittableTypes HitCheckType { get; private set; }

    [field: SerializeField] public HitDirections[] HitDirections { get; private set; }

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
    [ContextMenu("Add ShootGun")]
    public void AddShootGun()
    {
        effects.Add(new ShootOwnerGun());
    }
}

