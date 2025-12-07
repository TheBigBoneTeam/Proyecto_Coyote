using CombatEffect;
using UnityEngine;
using UnityEngine.UIElements;

public class LivingBullet : baseBullet
{
   public LayerMask layerWhenBullet;
    CapsuleCollider capsule;
    Rigidbody rb;
    public override void StartBulletMovement(AGameCharacter shooter, Vector3 spawnPoint, Vector3 objective)
    {
        capsule.enabled = false;
        rb.useGravity = false;

        gameObject.layer = LayerMask.NameToLayer("Default");
        if (GetComponent<EnemyAI>().isLocked())
        {
            FindAnyObjectByType<EnemyLockOn>().ResetTarget();
        }
        base.StartBulletMovement(shooter, spawnPoint, objective);
        GetComponent<BombEnemyAssetBehaviourRunner>().Fly();
        GetComponent<BombEnemyAssetBehaviourRunner>().enabled = false;
    }

    protected override void Awake()
    {
        base.Awake();
        capsule = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        Bulcollider = GetComponent<SphereCollider>();
        rb.useGravity = true;
        Bulcollider.enabled = false;
        capsule.enabled = true;
    }

    public override void destroyFunc()
    {
        //sphereCollider.enabled = false;
        //capsule.enabled = true;
        //rb.useGravity = true;
        print("DestroyBomb");
        gameObject.layer = LayerMask.NameToLayer("Enemy");

      //  rb.constraints = RigidbodyConstraints.FreezePosition;
        GetComponent<Enemy>().Die();
    }
    public override void restart()
    {
        Bulcollider.enabled = false;
        capsule.enabled = true;
        rb.useGravity = true;

        //Si pones el restart se bugea lo pos
        //base.restart();
        //gameObject.SetActive(true);
        //transform.position = ogPosition;
        attackStateEvent.RemoveAllListeners();
    }
}