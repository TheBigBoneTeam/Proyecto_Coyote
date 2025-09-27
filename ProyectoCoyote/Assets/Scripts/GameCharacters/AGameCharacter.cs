using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class AGameCharacter :MonoBehaviour
{
    List<ACombatEffect> activeEffects;
    public int HealthPoint { get; private set; }
    private int _maxHealthPoint;

public virtual void getHit(int healthPoint)
    {
        HealthPoint -= healthPoint;
        if(healthPoint < 0)
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
    public void addEffect(ACombatEffect effect)
    {
        effect.Activate();
        if (!effect.instant)
        {
            activeEffects.Add(effect);
        }
    }
}
public abstract class ACombatEffect
{
    protected AGameCharacter character;

    public bool instant;
    protected float _duration;
    protected float _currentDuration;
    public abstract void Activate();
    public virtual bool Update()
    {
        _currentDuration -= Time.deltaTime;
        if(_currentDuration<= 0)
        {
            return true;
        }
        return false;
    }

    public abstract void End();
}
public class DamageEffect : ACombatEffect
{
    private int _damage;


    public  DamageEffect(AGameCharacter character, int damage)
    {
        instant = true;
        this.character = character;
        this._damage = damage;
    }
    public override void Activate()
    {
        character.getHit(_damage);
    }

    public override void End()
    {
        throw new System.NotImplementedException();
    }
}
public class StunEffect: ACombatEffect
{
    public StunEffect(AGameCharacter character, float duration)
    {
        this._duration = duration;        
        this.character = character;
    }
    public override void Activate()
    {
        character.gameObject.GetComponent<Renderer>().material.color = Color.yellow;


    }

    public override void End()
    {
        character.gameObject.GetComponent<Renderer>().material.color = Color.white;

    }
}
public abstract class ACombatEffectSource:MonoBehaviour
{
    private void Awake()
    {
        
    }

}
public class TouchCombatEffectSource : ACombatEffectSource
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() == typeof(AGameCharacter))
        {
            other.GetComponent<AGameCharacter>().addEffect(new StunEffect(,3));
        }
    }

}
