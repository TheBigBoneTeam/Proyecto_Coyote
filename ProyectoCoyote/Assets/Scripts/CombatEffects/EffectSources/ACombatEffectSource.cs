using CombatEffect;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CombatEffect
{
    public abstract class ACombatEffectSource : MonoBehaviour
    {
        [SerializeField] protected bool oneUse;
    [SerializeField]  [SerializeReference]   List<ACombatEffect> effects;
        [SerializeReference] public DamageEffect damageEffect;
        //[SerializeField] public  DamageEffect effes;
        [ContextMenu("Add Stun")]
        public void AddStun()
        {
            effects.Add(new StunEffect(this, 10));
        }
        [ContextMenu("Add Damage")]
        public void AddDamage()
        {
            effects.Add(new DamageEffect(this, 10));
        }
        protected void Awake()
        {

        }
        public virtual void addEffectsToObj(AGameCharacter charac)
        {
            foreach (var effect in effects) {
                charac.checkEffect(effect);
            }
            if (oneUse)
            {
                destroySource();
            }
        }
        
        protected abstract ACombatEffect createEffect(AGameCharacter character);

        protected virtual void destroySource()
        {
            Destroy(gameObject);
        }



    }
    public abstract class Attack :MonoBehaviour
    {
        public AGameCharacter owner { get; private set; }
        [field: SerializeField] public HittableTypes HitCheckType { get; private set; }
        AHittableCheck HitCheck;

        [field:SerializeField] public HitDirections[] HitDirections { get; private set; }
        protected void OnTriggerEnter(Collider other)
        {
            AGameCharacter character = other.GetComponent<AGameCharacter>();
            if (character)
            {
                //Comprueba si el personaje golpeado es golpeable
                if (HitCheck.isHittable(character))
                {
                    character.GetComponent<DamageReceiver>().checkEffectSource(this);
                }

            }
        }
        protected void Start()
        {
            setHitCheck(HitCheckType);
        }
        public void setOwner(AGameCharacter owner)
        {
            this.owner = owner;
        }
        public void setHitCheck(HittableTypes type)
        {
            switch (type)
            {
                case HittableTypes.allCharacters:
                    HitCheck = new AllCharacterHittable(owner);
                    break;
                case HittableTypes.allCharactersNoMe:
                    HitCheck = new AllCharacterNoMeHittable(owner);
                    break;
                case HittableTypes.onlyOtherTeam:
                    HitCheck = new OnlyOtherTeamHittable(owner);
                    break;
            }
        }

    }
}
public enum HitDirections
{
    Left,
    Rigth,
    Back,
}
public class DamageReceiver:MonoBehaviour
{
    AGameCharacter character;
    HitDirections direction;
    bool dodging;
   public void checkEffectSource(Attack aOwnerableEffectSource)
    {
        if (!dodging || aOwnerableEffectSource.HitDirections.Contains(direction))
        {
            //aOwnerableEffectSource.addEffectsToObj(character);
        }
    }
    private void Start()
    {
        character = GetComponent<AGameCharacter>();
    }
 
}
