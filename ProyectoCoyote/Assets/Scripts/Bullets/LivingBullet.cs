using UnityEngine;

public class LivingBullet : baseBullet
{
   public LayerMask layerWhenBullet;
    public override void StartBulletMovement(AGameCharacter shooter, Vector3 spawnPoint, Vector3 objective)
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        if (GetComponent<EnemyAI>().isLocked())
        {
            FindAnyObjectByType<EnemyLockOn>().ResetTarget();
        }
        base.StartBulletMovement(shooter, spawnPoint, objective);
        GetComponent<BombEnemyAssetBehaviourRunner>().Fly();
    }
}