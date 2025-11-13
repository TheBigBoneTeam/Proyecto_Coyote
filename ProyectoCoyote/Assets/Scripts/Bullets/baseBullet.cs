using CombatEffect;
using System;
using UnityEngine;

public class baseBullet : Attack, IBullet
{
    [SerializeField] Vector3 objective;
    [SerializeField] public float speed;
    [SerializeField] float lifeTime;

    [SerializeField] bool Infinite;

    public bool hit;

    [SerializeField] protected ParticleSystem hitParticle;
    [SerializeField] protected ParticleSystem impactParticle;

    Action<baseBullet> onFire;


   protected combatAreaManager areaManager;



    [SerializeField] protected Animator anim;

 [SerializeField]   bool flying;
    protected override void OnTriggerEnter(Collider other)
    {
        print("BulletTrigger" + other.name);
        AGameCharacter character = other.GetComponent<AGameCharacter>();
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
                Destroy(gameObject, 0.5f);

            }
        } else if (!other.isTrigger)
        {
            print("BulletTriggerWall" + other.name);

            foreach (ACombatEffect effect in effects)
            {
                effect.Activate(null);
            }
            flying = false;
            Destroy(gameObject, 0.5f);
        }
        print("??");

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
                    Destroy(gameObject);
                }
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        //transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
   
    public virtual void StartBulletMovement(AGameCharacter shooter, Vector3 spawnPoint,Vector3 objective)
    {
        LoadData(_attackData);
        setOwner(shooter);
       // setHitCheck(HittableTypes.onlyOtherTeam);
        GetComponent<Collider>().isTrigger = true;
        transform.position = spawnPoint;
        print(objective);
        this.objective = objective;
        print(this.objective);
        transform.LookAt(objective);
        flying = true; 
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
