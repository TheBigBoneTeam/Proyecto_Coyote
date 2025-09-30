using UnityEngine;

namespace CombatEffect
{
    public class DamageSource : ATouchCombatEffectSource
    {
        [SerializeField] private int _damage;

        protected override ACombatEffect createEffect(AGameCharacter character)
        {
            return new DamageEffect(this,_damage);
        }
    }
}
