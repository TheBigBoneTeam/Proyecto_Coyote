using BehaviourAPI.Core;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class BullEnemyAssetBehaviourRunner : EnemyAssetBehaviourRunner
{
  [SerializeField]  int ammo;
  public  baseBullet currentAmmo;
    public bool hasAmmo;

    [SerializeField] float meleeDistance;
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