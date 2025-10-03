using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using CombatEffect;
using System;
public abstract class AGameCharacter :MonoBehaviour
{
    List<ACombatEffect> activeEffects;
   [field:SerializeField] public int HealthPoint { get; private set; }
   [SerializeField] private int _maxHealthPoint;
    [SerializeField] bool inmuneStun;
    private void Awake()
    {
        activeEffects = new List<ACombatEffect>();
    }
    private void Start()
    {
        HealthPoint = _maxHealthPoint;
    }
    public virtual void getHit(int damage)
    {
        HealthPoint -= damage;
        print($"{name} Recibido daño {damage} Vida actual {HealthPoint}");
        if(HealthPoint <= 0)
        {
            Die();
        }
    }

    public abstract void Die();
    private void Update()
    {
        
        foreach (var effect in activeEffects.ToArray())
        {
            if (effect.Update()){
                effect.End();
                activeEffects.Remove(effect);
            }
        }
    }
    protected virtual void addEffect(ACombatEffect effect)
    {
        effect.Activate(this);
        if (!effect.Instant)
        {
            activeEffects.Add(effect);
        }
    }

    public virtual bool checkEffect(ACombatEffect effect)
    {
        //Comprobacion de inmunidad mas compleja
        if (inmuneStun && effect.GetType() == typeof(StunEffect))
        {
            addEffect(new fakeStunEffect((StunEffect)effect));
            return false;
        }
        addEffect(effect);
        return true;
    }

    public virtual bool isOtherTeam(AGameCharacter character)
    {
        return false;
    }

   
}
