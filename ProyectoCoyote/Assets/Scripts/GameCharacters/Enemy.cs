using CombatEffect;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class Enemy : AGameCharacter
{
    combatAreaManager combatArea;
    [SerializeField] bool ActiveBeforeFight;
    [SerializeField] GameObject HitParticles;
    public combatAreaManager CombatArea { get; private set; }
    bool setredUp;
    bool dead;

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
        base.getHit(damage,directions, crit);
        GetComponent<Animator>().Play("heavySquish");
        if(HitParticles != null)
        {
            HitParticles.transform.position += new Vector3(UnityEngine.Random.Range(-.5f, .5f), UnityEngine.Random.Range(-.2f, .2f), UnityEngine.Random.Range(-.5f, .5f));
            foreach(ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
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
        print(name);
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
}
