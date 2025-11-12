using UnityEngine;
using UnityEngine.TextCore.Text;
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
            if (objCharacter && this.objCharacter.GetComponent<Player>() != null)
            {
                objCharacter.gameObject.GetComponentInChildren<Renderer>().material.color = Color.yellow;
                objCharacter.gameObject.GetComponentInParent<PlayerMovement>().setCanMove(false);
                objCharacter.gameObject.GetComponentInParent<PlayerMovement>().setCanAttack(false);
                objCharacter.gameObject.GetComponentInParent<EnemyLockOn>().ResetTarget();
            }
        }

        public override void End()
        {
            Debug.Log("EndStun");

            objCharacter.gameObject.GetComponentInChildren<Renderer>().material.color = Color.gray;
            objCharacter.gameObject.GetComponentInParent<PlayerMovement>().setCanMove(true);
            objCharacter.gameObject.GetComponentInParent<PlayerMovement>().setCanAttack(true);

        }

        public override float getDuration() => stunDuration;
    }
}