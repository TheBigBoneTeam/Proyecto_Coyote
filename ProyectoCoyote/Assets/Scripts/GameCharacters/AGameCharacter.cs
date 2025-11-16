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
   [SerializeField] protected int _maxHealthPoint;
    [SerializeField] bool inmuneStun;
    [SerializeField] float invTimeAfterHit = 1;
  [SerializeField]  protected bool invincible;
    Animator anim;

    UnityEvent<int> lifeUpdate;
   protected UnityEvent<AGameCharacter> dieEvent;
    UnityEvent<HitDirections> dodgeAttackEvent;

  [SerializeField] protected Vector3 startPos;

    [SerializeField]  bool print;
    [SerializeField] Renderer renderer;
    public Attack attack { get; private set; }


    string currentAnim;
    private void Awake()
    {
        lifeUpdate = new UnityEvent<int>();
        activeEffects = new List<ATimedEffect>();
        anim = GetComponentInChildren<Animator>();
        dodgeAttackEvent = new UnityEvent<HitDirections>();
        dieEvent = new UnityEvent<AGameCharacter>();
        attack = GetComponentInChildren<Attack>();
        if (renderer == null)
        {
            renderer= GetComponentInChildren<Renderer>();
        }
    }
    protected virtual void Start()
    {
        startPos = transform.position;
    }
    public virtual void getHit(int damage, HitDirections direction,bool crit = false)
    {
        HealthPoint -= damage;
        //print($"{name} Recibido daño {damage} Vida actual {HealthPoint}");
        lifeUpdate.Invoke(HealthPoint);
        AudioManager.Instance.PlaySimpleSound("SFX - Punch", false, Vector2.zero, true, false);
        string extra = "";
        switch (direction)
        {
            case HitDirections.Left:
                extra = "L";
                break;
            case HitDirections.Rigth:
                extra = "R";

                break;
            case HitDirections.Back:
                extra = "M";

                break;
            case HitDirections.Outside:
                extra = "M";
                break;
            default:
                extra = "M";
                break;
        }

        if (HealthPoint <= 0)
        {
            Die();
            return;
        }
        if (crit)
        {
            anim.CrossFade("GetHit"+extra, .1f, 0, 0);
            AudioManager.Instance.PlaySimpleSound("SFX - Crit", false, Vector2.zero, true, false);
            // anim.CrossFade("GetHit_CRIT"+extra, .1f, 0, 0);
        }
        else
        {
            print("GetHit" + extra);
            anim.CrossFade("GetHit" + extra, .1f, 0, 0);
            }
        if (invTimeAfterHit > 0)
        {
            invincible = true;
            StartCoroutine(ResetInvincible(invTimeAfterHit));
        }
    }
    public virtual void getHealed(int points)
    {
        HealthPoint += points;
        lifeUpdate.Invoke(HealthPoint);
    }
    public void setHealthPoint(int points)
    {
        HealthPoint = points;
        lifeUpdate.Invoke(HealthPoint);
    }
    IEnumerator ResetInvincible(float time)
    {
        float timepass = 0;

        while ((timepass<time))
        {
            yield return new WaitForSeconds(0.1f);
            renderer.enabled = !renderer.enabled;
            timepass+=0.1f;

        }
        renderer.enabled = true;

        invincible = false;
    }

    public virtual void restart()
    {
        currentAnim = "";
       // print("restart" + name);
        HealthPoint = _maxHealthPoint;
        lifeUpdate?.Invoke(HealthPoint);
        transform.position = startPos;
        dodgeAttackEvent ??= new UnityEvent<HitDirections>();
        dieEvent ??= new UnityEvent<AGameCharacter>();
        //print("restartposition" + name + startPos+transform.position);

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
        ////Comprobacion de inmunidad mas compleja
        //if (inmuneStun && effect.GetType() == typeof(StunEffect))
        //{
        //    addEffect(new fakeStunEffect((StunEffect)effect));
        //    return false;
        //}
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
        dodgeAttackEvent?.Invoke(direction);
        checkEffect(new Dodge(2));
    }


    public void PlayAnimation(AnimationClip clip)
    {
        AnimationPlayableUtilities.PlayClip(anim, clip, out PlayableGraph graph);
        graph.Play();
    }
    public void PlayAnimation(string stateName, bool idle = false)
    {

        //if (stateName == "CombatIdle"&& "CombatIdle" == currentAnim)
        //{
        //   //anim.CrossFade(stateName,.1f);
        //}
        //else
        //{
        //    if (print)
        //    {
        //        Debug.Log("stateName" + stateName);

        //    }
            //currentAnim = stateName;
            anim.CrossFade(stateName,.1f, 0, 0);
       // }
    }
    public void subscribeToLifeChange(UnityAction<int> response)
    {
        lifeUpdate.AddListener(response);
        response(HealthPoint);

    }

    public void unSubscribeToLifeChange(UnityAction<int> response)
    {
        lifeUpdate.RemoveListener(response);
    }

    public void subscribeToDodgeAttack(UnityAction<HitDirections> response)
    {
        print(dodgeAttackEvent == null);
        dodgeAttackEvent.AddListener(response);
    }
    public void unSubscribeToDodgeAttack(UnityAction<HitDirections> response)
    {
        dodgeAttackEvent.RemoveListener(response);
    }

    public void subscribeToDie(UnityAction<AGameCharacter> response)
    {
        dieEvent?.AddListener(response);
    }
    public void unSubscribeToDie(UnityAction<AGameCharacter> response)
    {
        dieEvent?.RemoveListener(response);
    }
}
