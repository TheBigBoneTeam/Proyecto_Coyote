using BehaviourAPI.Core;
using System.ComponentModel;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements.Experimental;

public class BullEnemyAssetBehaviourRunner : EnemyAssetBehaviourRunner
{
    [SerializeField] int ammo;

    [SerializeField] baseBullet _currentAmmo;

    [field:SerializeField] public baseBullet _closestAmmo { get; private set; }
    [SerializeField] float _closestAmmoDistance;
    [SerializeField] float _distanceToPlayer;

    [SerializeField] LayerMask environmentLayer;

    [SerializeField] int frameIntervalForDistanceCalculation;

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
                    _currentAmmo.GetComponent<BombEnemyAssetBehaviourRunner>().currentHeavy = null;
                }
            }
            _currentAmmo = value;
            print("_CurrentAmmo Bull" + _currentAmmo);
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
    [field: SerializeField]
    public bool hasAmmo /*{ get; private set; }*/
           ;
    [SerializeField] float meleeDistance;
    public bool noAmmo() => !hasAmmo;
    public override void restart()
    {
        base.restart();
        _closestAmmoDistance = float.MaxValue;
        _closestAmmo = null;
        enemy.CombatArea.subscribeToAmmoChange(checkAmmoVoid);
    }
    protected override void OnDisable()
    {
        if (enemy && enemy.CombatArea)
        {

            enemy.CombatArea.unSubscribeToAmmoChange(checkAmmoVoid);
            if (currentAmmo != null && currentAmmo.owner == (AGameCharacter)enemy)
            {
                enemy.CombatArea.changeInAmmoOwnership();
            }
        }
        if (_currentAmmo != null)
        {
            if (_currentAmmo.GetComponent<BombEnemyAssetBehaviourRunner>() != null)
            {
                _currentAmmo.GetComponent<Enemy>().unSubscribeToDie(BombEnemyDie);
            }
            _currentAmmo = null;
        }
        base.OnDisable();
    }
    protected override void OnUpdated()
    {
        base.OnUpdated();

        if(gameStateManager.getState() != GameState.Combat)
        {
            return;
        }
        if (Time.frameCount % frameIntervalForDistanceCalculation == 0)
        {
            findClosestRock();
            _distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        }
    }
    void checkAmmoVoid()
    {
        checkAmmo();
    }
    public Status checkAmmo()
    {
        baseBullet[] bullets = enemy.CombatArea.getAllBullets();
        if (bullets == null || bullets.Length == 0)
        {
            hasAmmo = false;
            return Status.Failure;
        }
        hasAmmo = true;
        return Status.Success;
    }
    public float DistanceToPlayer() {
        float f = _distanceToPlayer;
        return f;
    
    }
    public float DistanceToClosestRock()
    {
        if (_closestAmmo != null)
        {
            return _closestAmmoDistance;
        }
        else
        {
            return int.MaxValue;
        }
    }

    public bool closeToPlayer()
    {
      return _distanceToPlayer <= meleeDistance;
    }
    public bool awayFromPlayer()
    {
        return _distanceToPlayer > meleeDistance;
    }
    public bool hasAnyAmmo()
    {
        return hasAmmo;
    }
    public float EnemyNum()
    {
        print("EnemyNum");

      return  player.GetCloseEnemies();
    }
 public   void findClosestRock()
    {
        print("FindRock");
        float currentDist = float.MaxValue;
        baseBullet bestAmmo = null;
        RaycastHit hit;
        Vector3 dir;
        //Primero se calcula el ammo actual por si es la mejor opcion
        if (currentAmmo != null)
        {
            dir = _currentAmmo.transform.position - player.transform.position;
            if (Physics.Raycast(player.transform.position, dir, out hit, dir.magnitude, environmentLayer))
            {

                if (hit.transform == player.transform)
                {
                    currentDist = Vector3.SqrMagnitude(_currentAmmo.transform.position - transform.position);
                    bestAmmo = _currentAmmo;
                }
            }
        }
        baseBullet[] allBullets = enemy.CombatArea.getAllBullets();
        if (allBullets != null)
        {
            foreach (baseBullet bul in allBullets)
            {
                if (bul.owner != null && bul.owner != enemy)
                {
                    continue;
                }
                dir = player.transform.position - bul.transform.position;
                float newDist = Vector3.SqrMagnitude(bul.transform.position - transform.position);
                Debug.DrawRay(player.transform.position, dir, Color.magenta, 2);

                print($"newDist{newDist}");
                if (bestAmmo == null || currentDist > newDist)
                {
                    print("TRYHIT");
                    if (Physics.Raycast(bul.transform.position, dir, out hit, dir.magnitude, environmentLayer))
                    {
                        print($"hit{hit.transform.name}");

                        if (hit.transform == player.transform)
                        {
                            currentDist = newDist;
                            bestAmmo = bul;
                        }
                    }
                }
            }
        }
        if (bestAmmo == null)
        {
            hasAmmo = false;
            _closestAmmoDistance = float.MaxValue;
            _closestAmmo = null;
            return;
        }
        hasAmmo = true;
        _closestAmmoDistance = Mathf.Sqrt(currentDist);
        _closestAmmo = bestAmmo;
    }

    void BombEnemyDie(AGameCharacter bombEnemy)
    {
        print("ammoDie");
        baseBullet bombrunner = bombEnemy.gameObject.GetComponent<baseBullet>();

        if (bombrunner && bombrunner == currentAmmo)
        {
            currentAmmo = null;
        }
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