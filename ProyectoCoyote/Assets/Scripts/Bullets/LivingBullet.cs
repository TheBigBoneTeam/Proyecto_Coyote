using UnityEngine;

public class LivingBullet : baseBullet
{
    public override void StartBulletMovement(AGameCharacter shooter, Vector3 spawnPoint, Vector3 objective)
    {
        base.StartBulletMovement(shooter, spawnPoint, objective);
        GetComponent<BombEnemyAssetBehaviourRunner>().Fly();
    }
}