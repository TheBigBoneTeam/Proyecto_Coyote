using UnityEngine;

namespace CombatEffect
{
    [System.Serializable]

    public class fakeStunEffect : StunEffect
    {
        public fakeStunEffect(StunEffect effect) : base(effect.source,effect.stunDuration)
        {
        }
        public fakeStunEffect(ACombatEffectSource source, float _duration) : base(source, _duration)
        {
        }

        public override void Activate(AGameCharacter character)
        {
            base.Activate(character);
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
    public class Dodge : StunEffect
    {
        public Dodge(StunEffect effect) : base(effect.source, effect.stunDuration)
        {
        }
        public Dodge(ACombatEffectSource source, float _duration) : base(source, _duration)
        {
        }
        public Dodge(float duration):base(duration) 
        {

        }

        public override void Activate(AGameCharacter character)
        {
            base.Activate(character);
            this.objCharacter = character;
            character.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }

        public override void End()
        {
            objCharacter.gameObject.GetComponent<Renderer>().material.color = Color.gray;

        }
    }
}