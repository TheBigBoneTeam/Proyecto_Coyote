using CombatEffect;
using UnityEngine;
using UnityEngine.UIElements;

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
        GetComponent<BombEnemyAssetBehaviourRunner>().enabled = false;
    }

    public override void destroyFunc()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        GetComponent<Enemy>().Die();
    }
    public override void restart()
    {
        //Si pones el restart se bugea lo pos
        //base.restart();
        //gameObject.SetActive(true);
        //transform.position = ogPosition;
        attackStateEvent.RemoveAllListeners();
    }
}