using System;
using UnityEngine;

public class SpawnableCactus : AGameCharacter,IPoolObject
{
    public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
    CactusSpawner spawner;
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
        instance.gameObject.SetActive(active);
        return instance;
    }

  
    internal void startAttack(Player player)
    {
        transform.LookAt(player.transform.position);
        anim.Play("Attack",0,0);
     
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
        spawner.destroyCactus(this);
    }
}
