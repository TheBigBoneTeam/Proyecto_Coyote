using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] protected  GameObject bullet;
   protected AGameCharacter gameCharacter;
    [SerializeField] protected Transform bulletSpawnPoint;
   protected Action<Attack.AttackState> shootAction;
   protected Attack.AttackState attackState;
    public virtual void Shoot(Vector3 obj, baseBullet bul = null)
    {
        if (bul == null) {
            GameObject bulet = Instantiate(bullet);
            bul = bulet.GetComponent<baseBullet>();
        }
        print("shoot");
        shootAction?.Invoke(attackState);

        bul.StartBulletMovement(gameCharacter, bulletSpawnPoint.position, obj);
    }
    protected virtual void Start()
    {
        if (bullet != null)
        {
            attackState = new Attack.AttackState(bullet.GetComponent<baseBullet>().HitDirections.ToArray());
        }
        gameCharacter = GetComponentInParent<AGameCharacter>();
    }
    public void subscribeToShoot(Action<Attack.AttackState> subscribe)
    {
        shootAction += subscribe;
    }
    public void unSubscribeToShoot(Action<Attack.AttackState> subscribe)
    {
        shootAction -= subscribe;
    }
}
