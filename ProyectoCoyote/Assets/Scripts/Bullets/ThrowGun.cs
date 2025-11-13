using UnityEngine;

public class ThrowGun : Gun
{
    public override void Shoot(Vector3 obj, baseBullet bul = null)
    {
        //attackState = new Attack.AttackState(GetComponent<BullEnemyAssetBehaviourRunner>().currentAmmo.HitDirections.ToArray(),);
        base.Shoot(obj, GetComponent<BullEnemyAssetBehaviourRunner>().currentAmmo);
    }
  
}