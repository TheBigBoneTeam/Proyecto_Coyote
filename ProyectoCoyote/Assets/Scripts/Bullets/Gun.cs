using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    AGameCharacter gameCharacter;
    [SerializeField] Transform bulletSpawnPoint;
    Action<Attack.AttackState> shootAction;
    Attack.AttackState attackState;
    public void Shoot(Vector3 obj)
    {
      GameObject bulet =  Instantiate(bullet);
        print("shoot");
        shootAction?.Invoke(attackState);

        bulet.GetComponent<baseBullet>().StartBulletMovement(gameCharacter, bulletSpawnPoint.position, obj);
    }
    private void Start()
    {
        attackState = new Attack.AttackState(bullet.GetComponent<baseBullet>().HitDirections.ToArray());
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