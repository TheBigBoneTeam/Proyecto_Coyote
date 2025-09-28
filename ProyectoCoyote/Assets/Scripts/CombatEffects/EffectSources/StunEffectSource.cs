using UnityEngine;

public class StunEffectSource : TouchCombatEffectSource
{
    [SerializeField] private float _duration;
    protected override void addEffectTo(AGameCharacter character)
    {
        character.checkEffect(new StunEffect(_duration));
    }

}
