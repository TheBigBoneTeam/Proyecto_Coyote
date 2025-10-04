using UnityEngine;
namespace CombatEffect
{
    [System.Serializable]
    public class StunEffect : ATimedEffect
    {
        public float stunDuration;
        public StunEffect(ACombatEffectSource source,float duration) : base(source,duration)
        {
            
        }
     public StunEffect(float duration)
        {
            stunDuration = duration;
        }
        public override void Activate(AGameCharacter character)
        {
            base.Activate(character);
            Debug.Log($"StartStun with duration of {stunDuration}");
            this.objCharacter = character;
            character.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }

        public override void End()
        {
            Debug.Log("EndStun");

            objCharacter.gameObject.GetComponent<Renderer>().material.color = Color.gray;

        }

        public override float getDuration() => stunDuration;
    }
}