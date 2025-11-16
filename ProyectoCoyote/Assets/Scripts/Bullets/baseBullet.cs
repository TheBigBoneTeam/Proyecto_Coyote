using CombatEffect;
using System;
using UnityEngine;

public class baseBullet : Attack, IBullet
{
    [SerializeField] Vector3 objective;
    [SerializeField] public float speed;
    [SerializeField] protected float lifeTime;

    [SerializeField] protected bool Infinite;

    public bool hit;

    [SerializeField] protected ParticleSystem hitParticle;
    [SerializeField] protected ParticleSystem impactParticle;

    protected Action<baseBullet> onFire;
    protected Action<baseBullet> beDestroy;


    protected combatAreaManager areaManager;



    [SerializeField] protected Animator anim;

 [SerializeField] protected  bool flying;
    protected override void OnTriggerEnter(Collider other)
    {
        print("BulletTrigger" + other.name);
        AGameCharacter character = other.GetComponent<AGameCharacter>();
        if (!flying)
        {
            return;
        }
        if (character)
        {
            if (HitCheck == null)
            {
                setHitCheck(HitCheckType);
            }

            //Comprueba si el personaje golpeado es golpeable
            if (this.HitCheck.isHittable(character))
            {
                character.GetComponent<DamageReceiver>().checkEffectSource(this);
                flying = false;
                if (GetComponentInChildren<MeshRenderer>())
                GetComponentInChildren<MeshRenderer>().enabled = false;
                beDestroy?.Invoke(this);
                destroyFunc();

            }
        } else if (!other.isTrigger)
        {
            print("BulletTriggerWall" + other.name);

            foreach (ACombatEffect effect in effects)
            {
                effect.Activate(null);
            }
            flying = false;
            beDestroy?.Invoke(this);
            destroyFunc();
        }
        print("??");

    }
    public virtual void destroyFunc()
    {
        Destroy(gameObject, 0.5f);

    }
    private void Update()
    {
        if (flying)
        {
            if (!Infinite)
            {
                lifeTime -= Time.deltaTime;
                if (lifeTime <= 0)
                {
                    beDestroy?.Invoke(this);
                    destroyFunc();
                }
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        //transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
   
    public virtual void StartBulletMovement(AGameCharacter shooter, Vector3 spawnPoint,Vector3 objective)
    {
        setOwner(shooter);
        print("shooter"+ shooter);
        LoadData(_attackData);
       // setHitCheck(HittableTypes.onlyOtherTeam);
        GetComponent<Collider>().isTrigger = true;
        transform.position = spawnPoint;
        print(objective);
        this.objective = objective;
        print(this.objective);
        transform.LookAt(objective);
        flying = true;
        print(this.owner);
        onFire?.Invoke(this);
        GetComponent<Collider>().enabled = true;
        
        if (anim)
        {
            anim.enabled = true;
            anim.Play("fly",0,0);
        }

    }

    
    public void subcribeToShoot(Action<baseBullet> response)
    {
        onFire += response;
        
    }
    public void unSubcribeToShoot(Action<baseBullet> response)
    {
        onFire -= response;
    }
    public void subscribeToDestroy(Action<baseBullet> response)
    {
        beDestroy += response;
    }
    public void unSubscribeToDestroy(Action<baseBullet> response)
    {
        beDestroy -= response;
    }
    public void setAreaManager(combatAreaManager combatAreaManager)
    {
        areaManager = combatAreaManager;
    }
    protected override void Start()
    {
        anim = GetComponentInChildren<Animator>();
      

    }

        private void OnDestroy()
    {
      

    }


    public GameObject getObj() => gameObject;

}
