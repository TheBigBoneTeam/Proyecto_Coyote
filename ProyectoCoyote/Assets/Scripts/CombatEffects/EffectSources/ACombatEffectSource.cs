using UnityEngine;
namespace CombatEffect
{
    public abstract class ACombatEffectSource : MonoBehaviour
    {
        [SerializeField] protected bool oneUse;
        private void Awake()
        {

        }
        protected virtual void addEffect(ACombatEffect effect)
        {
            effect.getCharacter().checkEffect(effect);
        }
        protected abstract ACombatEffect createEffect(AGameCharacter character);

        protected virtual void destroySource()
        {
            Destroy(gameObject);
        }



    }
    public abstract class AOwnerableEffectSource : ATouchCombatEffectSource
    {
        public AGameCharacter owner { get; private set; }
        [field: SerializeField] public HittableTypes HitCheckType { get; private set; }
        AHittableCheck HitCheck;

        protected override void OnTriggerEnter(Collider other)
        {
            AGameCharacter character = other.GetComponent<AGameCharacter>();
            if (character)
            {
                //Comprueba si el personaje golpeado es golpeable
                if (HitCheck.isHittable(character))
                {
                    addEffect(createEffect(character));
                }

            }
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
