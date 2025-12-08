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

 [SerializeField]   bool shouldNotBeDestroyed = false;
    Vector3 ogPosition;

    Cover cover;

    [SerializeField] protected Animator anim;

 [SerializeField] protected  bool flying;

 protected   Collider Bulcollider;
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
                //if (GetComponentInChildren<MeshRenderer>())
                //GetComponentInChildren<MeshRenderer>().enabled = false;
                print("DestroyBul"+ other.name);
                beDestroy?.Invoke(this);
                destroyFunc();

            }
        } else if (!other.isTrigger && isThisNotCover(other.transform))
        {
            print("BulletTriggerWall" + other.name);

            foreach (ACombatEffect effect in effects)
            {
                effect.Activate(null);
            }
            flying = false;
            print("DestroyBul");
            beDestroy?.Invoke(this);
            destroyFunc();
        }
        print("??");

    }
    public bool isThisNotCover(Transform obj)
    {
        if (cover == null)
            return true;
        if (obj.transform.Equals(cover.transform))
        {
            return false;
        }
        if(obj.transform.parent == null)
        {
            return true;
        }
        if(obj.transform.parent.Equals(cover.transform))
        {
            return false;
        }
        return true;
    }
    public virtual void destroyFunc()
    {
        if(shouldNotBeDestroyed){
            gameObject.SetActive(false);
            return;
        }
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
                    print("DestroyBul");
                    destroyFunc();
                }
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        //transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
   
    public virtual void StartBulletMovement(AGameCharacter shooter, Vector3 spawnPoint,Vector3 objective)
    {
        if (shooter.GetComponent<DistanceEnemyAssetBehaviourRunner>())
        {
            cover = shooter.GetComponent<DistanceEnemyAssetBehaviourRunner>().currentCover;
        }
        else
        {
            cover = null;
        }
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
        Bulcollider.enabled = true;
        
        if (anim)
        {
            anim.enabled = true;
            anim.Play("fly",0,0);
        }

    }

    public override void restart()
    {
        base.restart();
        gameObject.SetActive(true);
        Bulcollider.enabled=false;
        transform.position = ogPosition;
    }
    protected override void Awake()
    {
        base.Awake();
        ogPosition = transform.position;    
        Bulcollider = gameObject.GetComponent<Collider>();
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
        shouldNotBeDestroyed = true;
        areaManager = combatAreaManager;
    }
    protected override void Start()
    {
        anim = GetComponentInChildren<Animator>();
      

    }


    public GameObject getObj() => gameObject;

}
