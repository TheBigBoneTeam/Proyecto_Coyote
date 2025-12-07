using CombatEffect;
using JetBrains.Annotations;
using Services;
using System;
using System.Collections;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : AGameCharacter
{
    combatAreaManager combatArea;
    [SerializeField] protected bool ActiveBeforeFight;
    [SerializeField] GameObject HitParticles, blockParticles, critParticles, blockParticlesPosition;
    Transform initialParticleTransform;
    DamageReceiver damageReceiver;
    [SerializeField] protected int healthDrop;
    public combatAreaManager CombatArea { get; private set; }
    bool setredUp;
  [field:SerializeField]  public bool dead { get; private set; }

    protected override void Start()
    {
        base.Start();
        damageReceiver = GetComponent<DamageReceiver>();
        initialParticleTransform = HitParticles.transform;
    }
    public override void Die()
    {
        if (!dead)
        {
            dead = true;
            print("dieEnemy"+name);
            dieEvent?.Invoke(this);
            GetComponent<EnemyAssetBehaviourRunner>().enabled = false;
          //  PlayAnimation("Die");
            FinishDie();
          //  gameObject.SetActive(false);
        }
    }
    public virtual void FinishDie()
    {
        gameObject.SetActive(false);
        ServiceLocator.Instance.Get<IHealthSpawner>().spawnOrb(transform.position, healthDrop);
    }
    IEnumerator DieAnim(float time)
    {
        float timepass = 0;

        while ((timepass < time))
        {
            yield return new WaitForSeconds(0.1f);
            foreach (Renderer mesh in renderers)
            {

                mesh.enabled = !mesh.enabled;
            }
            timepass += 0.1f;

        }
        foreach (Renderer mesh in renderers)
        {

            mesh.enabled = true;
        }
        FinishDie();
    }
    public override bool isOtherTeam(AGameCharacter character)
    {
        print(character.name);
        print(character.GetComponent<Enemy>() == null);
        return character.GetComponent<Enemy>() == null;
    }
    public override void getHit(int damage, HitDirections directions, bool crit = false)
    {
        damageReceiver.setDodge(false);
        base.getHit(damage,directions, crit);
        GetComponent<Animator>()?.Play("heavySquish");
        GetComponent<HitStopComponent>()?.HitStop(.075f);
        if (!crit)
        {
            if (HitParticles != null)
            {
                HitParticles.transform.localPosition = initialParticleTransform.localPosition + new Vector3(UnityEngine.Random.Range(-.2f, .2f), UnityEngine.Random.Range(-.2f, .2f), UnityEngine.Random.Range(-.2f, .2f));
                foreach (ParticleSystem particle in HitParticles.GetComponentsInChildren<ParticleSystem>())
                {
                    if (!particle.isPlaying)
                        particle.Play();
                }
            }
        }else
        { if (critParticles != null)
            {
                critParticles.transform.localPosition = initialParticleTransform.localPosition + new Vector3(UnityEngine.Random.Range(-.2f, .2f), UnityEngine.Random.Range(-.2f, .2f), UnityEngine.Random.Range(-.2f, .2f));
                foreach (ParticleSystem particle in critParticles.GetComponentsInChildren<ParticleSystem>())
                {
                    if (!particle.isPlaying)
                        particle.Play();
                }
            } 
        }

    }

 



    
    

    public void setArea(combatAreaManager combatArea)
    {
        CombatArea =combatArea;
    }
    public override void restart()
    {
        if (!setredUp)
        {
            startPos = transform.position;
            setredUp = true;
        }
        dead = false;
        dieEvent?.RemoveAllListeners();
        base.restart();
        gameObject.SetActive(ActiveBeforeFight);
        GetComponent<EnemyAI>().restart();
        if (GetComponent<HookableObject>() != null)
        {
            GetComponent<HookableObject>().restart();

        }

        GetComponentInChildren<Attack>().restart();

        GetComponent<EnemyAssetBehaviourRunner>().enabled = false;

    }
    public virtual void activateEnemy(bool active)
    {
        print($"activateEnemy{name} {active}");
        gameObject.SetActive(ActiveBeforeFight ? true:active);
        GetComponent<EnemyAssetBehaviourRunner>().enabled = active;
        if (active)
        {
            GetComponent<EnemyAssetBehaviourRunner>().restart();
        }
    }

    public override void DodgeAttack(HitDirections direction)
    {
        base.DodgeAttack(direction);
        GetComponentInChildren<SkinnedMeshRenderer>().material.SetColor("_HitColor", Color.blue);
        GetComponentInChildren<SkinnedMeshRenderer>().material.SetFloat("hitTransparency", .45f);
        GetComponentInChildren<SkinnedMeshRenderer>().material.SetInt("_isHit", 1);
        GetComponent<Animator>()?.Play("lightSquish");
        AudioManager.Instance.PlaySimpleSound("SFX - Block Attack", false, Vector2.zero, true, false);
        StartCoroutine("ResetMaterialHit");
        if (blockParticles != null && blockParticlesPosition != null)
        {
            /*float xadd = 0;
            switch(direction)
            {
                case HitDirections.Left:
                    xadd = -1f;
                    break;
                case HitDirections.Rigth:
                    xadd = 1f;
                    break;
                default:
                    xadd = 0f;
                    break;
            }
            */
            blockParticles.transform.position = blockParticlesPosition.transform.position;
            
            foreach (ParticleSystem particle in blockParticles.GetComponentsInChildren<ParticleSystem>())
            {
                if (!particle.isPlaying)
                    particle.Play();
            }
        }
    }
    
}

