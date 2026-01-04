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
 protected   Rigidbody rb;

    List<ATimedEffect> activeEffects;
   [field:SerializeField] public int HealthPoint { get; private set; }
   [field:SerializeField] public int _maxHealthPoint { get; private set; }
    [SerializeField] bool inmuneStun;
    [SerializeField] float invTimeAfterHit = 1;
  [SerializeField]  protected bool invincible;
    [SerializeField] protected Animator anim;

    UnityEvent<int> lifeUpdate;
   protected UnityEvent<AGameCharacter> dieEvent;
    UnityEvent<HitDirections> dodgeAttackEvent;

  [SerializeField] public Vector3 startPos;

    [SerializeField]  bool shouldprint;
    [SerializeField] protected Renderer[] renderers;
  [field:SerializeField]  public Attack attack { get; private set; }

    [SerializeField] ParticleSystem healparticles;


    string currentAnim;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lifeUpdate = new UnityEvent<int>();
        activeEffects = new List<ATimedEffect>();
        //anim = GetComponentInChildren<Animator>();
        dodgeAttackEvent = new UnityEvent<HitDirections>();
        dieEvent = new UnityEvent<AGameCharacter>();
        attack = GetComponentInChildren<Attack>();
      
            renderers= GetComponentsInChildren<SkinnedMeshRenderer>();
        
        print($"setStartPos{name}{transform.localPosition}");
        //startPos = transform.position;
    }
    protected virtual void Start()
    {

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
            foreach (SkinnedMeshRenderer mesh in renderers)
            {
                mesh.material.SetColor("_HitColor", Color.red);
                mesh.material.SetFloat("hitTransparency", .2f);
                mesh.material.SetInt("_isHit", 1);
            }
            StartCoroutine("ResetMaterialHit");
            anim.CrossFade("GetHit" + extra, .1f, 0, 0);
            }
        if (invTimeAfterHit > 0)
        {
            invincible = true;
            StartCoroutine(ResetInvincible(invTimeAfterHit));
        }
    }
    public IEnumerator ResetMaterialHit()
    {
        yield return new WaitForSeconds(.3f);
        foreach (Renderer mesh in renderers)
        {
            mesh.material.SetInt("_isHit", 0);
        }
    }

    public virtual void getHealed(int points)
    {
        healparticles.Play();
        HealthPoint = Mathf.Min( HealthPoint + points,_maxHealthPoint);
        lifeUpdate.Invoke(HealthPoint);

        if (points == 1) AudioManager.Instance.PlaySimpleSound("SFX - Vida 1", false, Vector2.zero, true, false);
        if (points == 2) AudioManager.Instance.PlaySimpleSound("SFX - Vida 2", false, Vector2.zero, true, false);
        if (points == 3) AudioManager.Instance.PlaySimpleSound("SFX - Vida 3", false, Vector2.zero, true, false);
        if (points == 4) AudioManager.Instance.PlaySimpleSound("SFX - Vida 4", false, Vector2.zero, true, false);
        if (points == 5) AudioManager.Instance.PlaySimpleSound("SFX - Vida 5", false, Vector2.zero, true, false);
        if (points == 6) AudioManager.Instance.PlaySimpleSound("SFX - Vida 6", false, Vector2.zero, true, false);
        if (points == 7) AudioManager.Instance.PlaySimpleSound("SFX - Vida 7", false, Vector2.zero, true, false);
        if (points == 8) AudioManager.Instance.PlaySimpleSound("SFX - Vida 8", false, Vector2.zero, true, false);
        if (points == 9) AudioManager.Instance.PlaySimpleSound("SFX - Vida 9", false, Vector2.zero, true, false);
        if (points == 10) AudioManager.Instance.PlaySimpleSound("SFX - Vida 10", false, Vector2.zero, true, false);
    }
    public void setHealthPoint(int points)
    {
        HealthPoint = points;
        lifeUpdate.Invoke(HealthPoint);
    }
    IEnumerator ResetInvincible(float time)
    {
        float timepass = 0;

        while ((timepass < time))
        {
            yield return new WaitForSeconds(0.1f);
            foreach (Renderer mesh in renderers) { 

                mesh.enabled = !mesh.enabled;
        }
            timepass+=0.1f;

        }
        foreach (Renderer mesh in renderers)
        {

            mesh.enabled = true;
        }
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
    protected virtual void Update()
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

    public virtual void DodgeAttack(HitDirections direction)
    {
        print("dodge");
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
        Debug.Log($"{name} playing animation: {stateName}");
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
    public void PlayAnimationCut(string stateName)
    {
        anim.Play(stateName);

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
