using Services;
using UnityEngine;

public class TutorialDefenseAttackUIIndicator:DefenseAttackUIIndicator
{
    [SerializeField] Enemy[] tutorialEnemies;

    public void restartTut()
    {
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
                    print(enemy.name);
                    enemy.attack.subscribeToStateChange(AttackHappeneed);
                    enemy.subscribeToDie(enemyDie);
                }
            }
        }
    }
    protected override void Start()
    {
        base.Start();
    }
}