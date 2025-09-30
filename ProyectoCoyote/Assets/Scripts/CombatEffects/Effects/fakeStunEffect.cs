using UnityEngine;

namespace CombatEffect
{
    public class fakeStunEffect : StunEffect
    {
        public fakeStunEffect(StunEffect effect) : base(effect.source,effect.Duration)
        {
        }
        public fakeStunEffect(ACombatEffectSource source, float _duration) : base(source, _duration)
        {
        }
        public override void Activate(AGameCharacter character)
        {
            Debug.Log("StartFakeStun");
            this.objCharacter = character;
            character.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }

        public override void End()
        {
            Debug.Log("EndFakeStun");
            objCharacter.gameObject.GetComponent<Renderer>().material.color = Color.gray;

        }
    }
}