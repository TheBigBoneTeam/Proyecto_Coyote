using UnityEngine;

public class DamageSource : TouchCombatEffectSource
{
    [SerializeField] private int _damage;
    protected override void addEffectTo(AGameCharacter character)
    {
        character.checkEffect(new DamageEffect(_damage));
    }
}
