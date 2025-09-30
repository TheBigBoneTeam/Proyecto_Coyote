using UnityEngine;
namespace CombatEffect
{
    public class StunEffectSource : ATouchCombatEffectSource
    {
        [SerializeField] private float _duration;
      

        protected override ACombatEffect createEffect(AGameCharacter character)
        {
            return new StunEffect(this,_duration);
        }
    }
}
