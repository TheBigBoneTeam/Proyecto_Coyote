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
}
