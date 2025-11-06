using CombatEffect;
using UnityEngine;

public class baseBullet : Attack, IBullet
{
[SerializeField]    Vector3 objective;
    [SerializeField] public float speed;
    [SerializeField] float lifeTime;

    [SerializeField] bool Infinite;

    public bool hit;

    [SerializeField] protected ParticleSystem hitParticle;
    [SerializeField] protected ParticleSystem impactParticle;






    [SerializeField] LayerMask obstacleLayer;

    [SerializeField] protected Animator anim;

 [SerializeField]   bool flying;

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
        setHitCheck(HittableTypes.onlyOtherTeam);
        transform.position = spawnPoint;
        print(objective);
        this.objective = objective;
        print(this.objective);
        transform.LookAt(objective);
        flying = true; 
        if (anim)
        {
            anim.Play("fly");
        }

    }



    protected override void Start()
    {
        anim = GetComponentInChildren<Animator>();
      

    }

    //public virtual void hitSomething(GameObject obj)
    //{
    //    print("hit");
    //    //Animacion o algo
    //    hit = true;
    //    anim.Play("bulletDestroy");
    //    if (obj.GetComponent<CharacterLife>() != null)
    //    {
    //        if (hitParticle)
    //        {
    //            float bulletAngle = transform.eulerAngles.z;
    //            //float rad = bulletAngle * Mathf.Deg2Rad;
    //            //Vector2 bulletDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    //            var particles = obj.GetComponentInChildren<ParticleSystem>(true);
    //            if (particles != null)
    //            {
    //                particles.gameObject.SetActive(true);
    //                particles.Play();
    //                if (transform.eulerAngles.z < 10 && transform.eulerAngles.z > -10)
    //                {
    //                    particles.transform.eulerAngles = new Vector3(-25, -particles.transform.eulerAngles.y, -particles.transform.eulerAngles.z);
    //                }
    //                else

    //                    particles.transform.eulerAngles = new Vector3(transform.eulerAngles.z, -particles.transform.eulerAngles.y, -particles.transform.eulerAngles.z);
    //            }

    //            musicManager.Instance.PlaySoundPitch("snd_contacto_enemigo");
    //        }
    //        else
    //        {
    //            if (impactParticle)
    //                impactParticle.Play();
    //            musicManager.Instance.PlaySoundPitch("snd_contacto_obstaculo");
    //        }
    //        speed = 0;
    //        GetComponent<Collider2D>().enabled = false;
    //    }
    //    ServiceLocator.Instance.Get<IsoftLock>().checkAll();
    //    Destroy(gameObject, 0.5f);


    //}
  //  public CharacterLife.Team getTeam() => team;

   // public bool hurtAll() => canHurtAll;

  
    private void OnDestroy()
    {
      

    }


    public GameObject getObj() => gameObject;

}
