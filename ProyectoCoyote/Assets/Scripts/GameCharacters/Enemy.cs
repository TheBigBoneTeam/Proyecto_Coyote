using CombatEffect;
using JetBrains.Annotations;
using Services;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Enemy : AGameCharacter
{
    combatAreaManager combatArea;
    [SerializeField] bool ActiveBeforeFight;
    [SerializeField] GameObject HitParticles, blockParticles, blockParticlesPosition;
    Transform initialParticleTransform;
    DamageReceiver damageReceiver;
    public combatAreaManager CombatArea { get; private set; }
    bool setredUp;
    bool dead;

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
            dieEvent?.Invoke(this);
            GetComponent<EnemyAssetBehaviourRunner>().enabled = false;
            gameObject.SetActive(false);
        }
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
        if (HitParticles != null)
        {
            HitParticles.transform.localPosition = initialParticleTransform.localPosition + new Vector3(UnityEngine.Random.Range(-.2f, .2f), UnityEngine.Random.Range(-.2f, .2f), UnityEngine.Random.Range(-.2f, .2f));
            foreach(ParticleSystem particle in HitParticles.GetComponentsInChildren<ParticleSystem>())
            {
                if(!particle.isPlaying)
                    particle.Play();
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
        GetComponentInChildren<Attack>().restart();

        GetComponent<EnemyAssetBehaviourRunner>().enabled = false;

    }
    public void activateEnemy(bool active)
    {
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
