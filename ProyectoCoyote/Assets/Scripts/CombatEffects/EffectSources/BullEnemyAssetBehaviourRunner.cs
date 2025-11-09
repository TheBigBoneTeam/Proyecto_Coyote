using BehaviourAPI.Core;
using UnityEngine;

public class BullEnemyAssetBehaviourRunner : EnemyAssetBehaviourRunner
{
  [SerializeField]  int ammo;
  public  baseBullet currentAmmo;
    public bool hasAmmo;
    public bool noAmmo() => !hasAmmo;
    public override void restart()
    {
        base.restart();
   
    }
    
    public Status checkAmmo()
    {
        if (enemy.CombatArea.getAllBullets().Length == 0)
        {
            hasAmmo = true;
            return Status.Success;
        }
        hasAmmo = false;
        return Status.Failure;
    }
}