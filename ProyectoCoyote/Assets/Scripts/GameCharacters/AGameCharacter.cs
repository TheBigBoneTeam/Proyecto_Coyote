using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using CombatEffect;
using System;
using System.Collections;
using UnityEngine.Playables;
public abstract class AGameCharacter :MonoBehaviour
{
    List<ATimedEffect> activeEffects;
   [field:SerializeField] public int HealthPoint { get; private set; }
   [SerializeField] private int _maxHealthPoint;
    [SerializeField] bool inmuneStun;
    [SerializeField] float invTimeAfterHit = 1;
  [SerializeField]  bool invincible;
    Animator anim;
    private void Awake()
    {
        activeEffects = new List<ATimedEffect>();
        anim = GetComponentInChildren<Animator>();
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
            return;
        }
        if (invTimeAfterHit > 0)
        {
            invincible = true;
            StartCoroutine(ResetInvincible(invTimeAfterHit));
        }
    }
    IEnumerator ResetInvincible(float time)
    {
        float timepass = 0;
        MeshRenderer filter = GetComponent<MeshRenderer>();
        while ((timepass<time))
        {
            yield return new WaitForSeconds(0.1f);
            filter.enabled = !filter.enabled;
            timepass+=0.1f;

        }
        filter.enabled = true;

        invincible = false;
    }


    public abstract void Die();
    private void Update()
    {
        print("update");
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
        print("addefect" + effect.GetType().Name);
        if (!effect.Instant())
        {
            print("addefecttolist" + effect.GetType().Name);

            activeEffects.Add((ATimedEffect)effect);
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
        if(invincible && effect.GetType() == typeof(DamageEffect))
        {

            return false;
        }
        addEffect(effect);
        return true;
    }
    public virtual bool isOtherTeam(AGameCharacter character)
    {
        return false;
    }

    internal void DodgeAttack()
    {
        checkEffect(new Dodge(2));
    }

    internal void PlayAnimation(AnimationClip clip)
    {
        AnimationPlayableUtilities.PlayClip(anim, clip, out PlayableGraph graph);
        graph.Play();
    }
}
