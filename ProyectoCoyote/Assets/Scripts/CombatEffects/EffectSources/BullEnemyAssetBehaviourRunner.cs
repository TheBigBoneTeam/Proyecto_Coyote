using BehaviourAPI.Core;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class BullEnemyAssetBehaviourRunner : EnemyAssetBehaviourRunner
{
  [SerializeField]  int ammo;

    baseBullet _currentAmmo;
  public baseBullet currentAmmo
    {
        get
        {
            return _currentAmmo;
        }
        set
        {
            if (_currentAmmo != null)
            {
                if (_currentAmmo.GetComponent<BombEnemyAssetBehaviourRunner>() != null)
                {
                    _currentAmmo.GetComponent<Enemy>().unSubscribeToDie(BombEnemyDie);
                }
            }
            _currentAmmo = value;
            if (_currentAmmo != null)
            {
                if (_currentAmmo.GetComponent<BombEnemyAssetBehaviourRunner>() != null)
                {
                    _currentAmmo.GetComponent<Enemy>().subscribeToDie(BombEnemyDie);
                    _currentAmmo.GetComponent<BombEnemyAssetBehaviourRunner>().currentHeavy = this;
                }
            }
        }
    }
    public bool hasAmmo;

    [SerializeField] float meleeDistance;
    public bool noAmmo() => !hasAmmo;
    public override void restart()
    {
        base.restart();
        enemy.CombatArea.subscribeToAmmoChange(checkAmmoVoid);


    }
    private void OnDisable()
    {
        if (enemy && enemy.CombatArea)
        {
            
            enemy.CombatArea.unSubscribeToAmmoChange(checkAmmoVoid);
            if (currentAmmo != null && currentAmmo.owner == (AGameCharacter)enemy)
            {
                enemy.CombatArea.changeInAmmoOwnership();
            }
        }
    }
    void checkAmmoVoid()
    {
        checkAmmo();
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

    public bool closeToPlayer()
    {
      return  Vector3.Distance(player.transform.position, transform.position) <=meleeDistance;
    }
    public bool awayFromPlayer()
    {
        return Vector3.Distance(player.transform.position, transform.position) > meleeDistance;
    }
    public bool hasAnyAmmo()
    {
        return hasAmmo;
    }

    void BombEnemyDie(AGameCharacter bombEnemy)
    {
        currentAmmo = null;
    }
    #region Gizmos
    private void OnDrawGizmos()
    {
        //// Set the color with custom alpha.
        //Gizmos.color = new Color(1f, 0f, 0f, 1f); // Red with custom alpha

        //// Draw the sphere.
        //Gizmos.DrawSphere(transform.position, seeDistance);

        // Draw wire sphere outline.
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, meleeDistance);
    }
    #endregion
}