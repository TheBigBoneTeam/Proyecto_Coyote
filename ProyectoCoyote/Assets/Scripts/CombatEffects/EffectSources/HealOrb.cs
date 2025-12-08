using CombatEffect;
using Services;
using System.Collections;
using UnityEngine;


public class HealOrb : TouchCombatEffectSource, IPoolObject
{
    int health;
    playerMagnet magnet;
    IHealthSpawner spawner;
    public bool careAboutMaxHealth;
    Animator animator;
    [SerializeField] float livingTime;
    protected override void OnTriggerEnter(Collider other)
    {
        AGameCharacter character = other.GetComponent<AGameCharacter>();
        if (character)
        {
            Player player = character.GetComponent<Player>();
            if (player && (player.HealthPoint < player._maxHealthPoint || !careAboutMaxHealth || magnet.goTowardPlayer))
            {
                addEffectsToChar(character);
                spawner.returnOrb(this);
            }

        }
    }
    public bool Active
    {
        get => gameObject.activeSelf;
        set
        {
            gameObject.SetActive(value);
            if (value)
            {
                animator.Play("healOrb_spawn", 0, 0);
                StartCoroutine(waitToDie());
                
            }
            else
            {
                StopAllCoroutines();
            }
        }
    }

    public void Clean()
    {

    }

    public IPoolObject Clone(Transform parent = null, bool active = false)
    {

        var instance = parent is null ? Instantiate(this) : Instantiate(this, parent);
        instance.animator = instance.GetComponentInChildren<Animator>();
        instance.magnet = instance.GetComponentInChildren<playerMagnet>();
        instance.gameObject.SetActive(active);
        instance.spawner = ServiceLocator.Instance.Get<IHealthSpawner>();
        return instance;
    }
    IEnumerator waitToDie()
    {
        yield return new WaitForSeconds(livingTime);
        selfReturn();
        animator.Play("healOrb_despawn", 0, 0);
    }
    public void selfReturn()
    {
        spawner.returnOrb(this);
    }
    public void setHeal(int heal)
    {
        health = heal;
        effects.Clear();
        effects.Add(new HealEffect(this, heal));
    }
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
}

