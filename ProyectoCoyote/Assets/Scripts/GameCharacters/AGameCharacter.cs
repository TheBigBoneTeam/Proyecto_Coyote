using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using CombatEffect;
using System;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.Events;
public abstract class AGameCharacter :MonoBehaviour
{
    List<ATimedEffect> activeEffects;
   [field:SerializeField] public int HealthPoint { get; private set; }
   [SerializeField] private int _maxHealthPoint;
    [SerializeField] bool inmuneStun;
    [SerializeField] float invTimeAfterHit = 1;
  [SerializeField]  bool invincible;
    Animator anim;

    UnityEvent<int> lifeUpdate;
   protected Action dieEvent;
    UnityEvent<HitDirections> dodgeAttackEvent;

    private void Awake()
    {
        lifeUpdate = new UnityEvent<int>();
        activeEffects = new List<ATimedEffect>();
        anim = GetComponentInChildren<Animator>();
        dodgeAttackEvent = new UnityEvent<HitDirections>();
    }
    private void Start()
    {
        HealthPoint = _maxHealthPoint;
        lifeUpdate.Invoke(HealthPoint);
    }
    public virtual void getHit(int damage)
    {
        HealthPoint -= damage;
        print($"{name} Recibido daño {damage} Vida actual {HealthPoint}");
        lifeUpdate.Invoke(HealthPoint);

        if (HealthPoint <= 0)
        {
            Die();
            return;
        }
        if (invTimeAfterHit > 0)
        {
            anim.CrossFade("GetHit", .1f,0,0);
            invincible = true;
            StartCoroutine(ResetInvincible(invTimeAfterHit));
        }
    }
    IEnumerator ResetInvincible(float time)
    {
        float timepass = 0;
        Renderer filter = GetComponentInChildren<Renderer>();

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

    public void DodgeAttack(HitDirections direction)
    {
        if(dodgeAttackEvent != null)
        dodgeAttackEvent.Invoke(direction);
        checkEffect(new Dodge(2));
    }


    public void PlayAnimation(AnimationClip clip)
    {
            AnimationPlayableUtilities.PlayClip(anim, clip, out PlayableGraph graph);

        graph.Play();
    }
    public void PlayAnimation(string stateName, bool idle = false)
    {
        if (idle)
        {
            anim.CrossFade(stateName,.1f);
        }
        else
        {
            anim.CrossFade(stateName,.1f, 0, 0);
        }
    }
    public void subscribeToLifeChange(UnityAction<int> response)
    {
        lifeUpdate.AddListener(response);
        response(HealthPoint);

    }

    public void unSubscribeToLifeChange(UnityAction<int> response)
    {
        lifeUpdate.AddListener(response);
    }

    public void subscribeToDodgeAttack(UnityAction<HitDirections> response)
    {
        dodgeAttackEvent.AddListener(response);
    }
    public void unSubscribeToDodgeAttack(UnityAction<HitDirections> response)
    {
        dodgeAttackEvent.RemoveListener(response);
    }

    public void subscribeToDie(Action response)
    {
        dieEvent += response;
    }
    public void unSubscribeToDie(Action response)
    {
        dieEvent -= response;
    }
}
