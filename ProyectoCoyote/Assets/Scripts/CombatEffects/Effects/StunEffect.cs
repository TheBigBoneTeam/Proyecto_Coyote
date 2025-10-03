using UnityEngine;
namespace CombatEffect
{
    [System.Serializable]
    public class StunEffect : TimedEffect
    {
        public StunEffect(ACombatEffectSource source,float duration) : base(source,duration)
        {
        }
        public override void Activate(AGameCharacter character)
        {
            Debug.Log($"StartStun with duration of {Duration}");
            this.objCharacter = character;
            character.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }

        public override void End()
        {
            Debug.Log("EndStun");

            objCharacter.gameObject.GetComponent<Renderer>().material.color = Color.gray;

        }
    }
}