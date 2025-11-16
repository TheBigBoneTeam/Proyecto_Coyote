using UnityEngine;

public class TutorialDefenseAttackUIIndicator:DefenseAttackUIIndicator
{
    [SerializeField] Enemy[] tutorialEnemies;
    protected override void Start()
    {
        base.Start();
        middleDanger.SetActive(false);
        foreach (var enemy in tutorialEnemies)
        {
            BombEnemyAssetBehaviourRunner bombRunner = enemy.GetComponent<BombEnemyAssetBehaviourRunner>();
            if (bombRunner != null)
            {
                bombRunner.subscribeToCharge(chargeExplosion);
            }
            else
            {
                print("subcribe");
                Gun gun = enemy.GetComponent<Gun>();
                if (gun)
                {
                    gun.subscribeToShoot(shootGun);
                }
                else
                {
                    enemy.attack.subscribeToStateChange(AttackHappeneed);
                    enemy.subscribeToDie(enemyDie);
                }
            }
        }
    }
}