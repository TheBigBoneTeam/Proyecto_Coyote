using System;
using UnityEngine;

public class SpawnableCactus : AGameCharacter,IPoolObject
{
    public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
  [SerializeField]  CactusSpawner spawner;
    Attack.AttackState attackState;
    Attack.AttackState nullAttackState;

    public void Clean()
    {
        anim.Play("Idle");
        

    }
    public override bool isOtherTeam(AGameCharacter character)
    {
        return character.GetComponent<Player>() != null;
    }
    public IPoolObject Clone(Transform parent = null, bool active = false)
    {

      
        var instance = parent is null ? Instantiate(this) : Instantiate(this, parent);
        instance.spawner = FindAnyObjectByType<CactusSpawner>();
        instance.attackState = new Attack.AttackState(GetComponentInChildren<Attack>(), instance);
        instance.nullAttackState ??= new Attack.AttackState(null, instance);
        instance.gameObject.SetActive(active);
        return instance;
    }

  
    internal void startAttack(Player player)
    {

        transform.LookAt(player.transform.position);
        anim.Play("Attack",0,0);
        
        print(attackState == null);
        spawner.cactusAttack(attackState);
     
    }


    protected override void Start()
    {
        base.Start();
       
       spawner = FindAnyObjectByType<CactusSpawner>();

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Die()
    {
        print("die");
        spawner.cactusAttack(nullAttackState);
        spawner.destroyCactus(this);
    }
}
