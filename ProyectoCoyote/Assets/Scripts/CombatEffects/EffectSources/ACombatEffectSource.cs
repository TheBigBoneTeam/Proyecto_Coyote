using CombatEffect;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CombatEffect
{
    public abstract class ACombatEffectSource : MonoBehaviour
    {
    [SerializeField]  [SerializeReference]  protected List<ACombatEffect> effects;
        //[SerializeField] public  DamageEffect effes;
        [ContextMenu("Add Stun")]
        public void AddStun()
        {
            effects.Add(new StunEffect(this, 1));
        }
        [ContextMenu("Add Damage")]
        public void AddDamage()
        {
            effects.Add(new DamageEffect(this, 3));
        }
        [ContextMenu("Add Crit Damage")]

        public void AddCritDamage()
        {
            effects.Add(new CritDamageEffect(this, 3));
        }
        [ContextMenu("Add ShootGun")]
        public void AddShootGun()
        {
            effects.Add(new ShootOwnerGun());
        }
        public virtual void addEffectsToChar(AGameCharacter charac)
        {
            foreach (var effect in effects) {
                charac.checkEffect(effect);
            }
            //if (oneUse)
            //{
            //    destroySource();
            //}
        }
        

        protected virtual void destroySource()
        {
            Destroy(gameObject);
        }



    }
    
}
public enum HitDirections
{
    Left,
    Rigth,
    Back,
    Outside
}
