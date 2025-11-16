using NUnit.Framework;
using Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyManager: IEnemyManager
{
    ClassMutex<EnemyAI> enemyClassMutex;
    ClassMutex<EnemyAI> attackingEnemy;
    Transform kungFuCircle;
    OwnerableTransform[] mainKungFuPoints;
    OwnerableTransform[] secondaryKungFuPoints;
    Player player;
    public void Instantiate()
    {
        enemyClassMutex = new ClassMutex<EnemyAI>();
        attackingEnemy = new ClassMutex<EnemyAI>();
        player = GameObject.FindAnyObjectByType<Player>();
        
        kungFuCircle = UnityEngine.GameObject.FindGameObjectWithTag("KungFuCircle").transform;
        mainKungFuPoints = new OwnerableTransform[kungFuCircle.childCount];
        Debug.Log(mainKungFuPoints.Length);
        for (int i = 0; i < kungFuCircle.childCount; i++)
        {
            mainKungFuPoints[i] = new OwnerableTransform(kungFuCircle.GetChild(i));
        }
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeToRestart(()=> { attackingEnemy.clearMutex(); clearKungFuPoints(); });
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeCombatAreaChange((a,b) => { attackingEnemy.clearMutex(); clearKungFuPoints(); });


    }
    public Transform getPoint(int index, Enemy owner)
    {
        if (mainKungFuPoints[index].checkOwner(owner))
        {
            return mainKungFuPoints[index].transform;
        }
        Debug.Log("isNull");
        return null;
    }
    public bool returnPoint(int index, Enemy owner)
    {
        if(index < 0 || index  >= mainKungFuPoints.Length) return false;
        if (mainKungFuPoints[index].checkOwner(owner))
        {
            mainKungFuPoints[index].Owner = null;
            return true;
        }
        Debug.Log("isNull");      
        return false;

    }

    public Transform getClosestPoint(Enemy owner, out int index)
    {
        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = owner.transform.position;

        int zOption = (enemyPos.z > playerPos.z) ? 0 : 2;
        int xOption = (enemyPos.x > playerPos.x) ? 1 : 3;
        
        if(Mathf.Abs(enemyPos.z) >= Mathf.Abs(enemyPos.x)){
            index = zOption;
            if (mainKungFuPoints[index].checkOwner(owner))
            {
                return mainKungFuPoints[index].transform;
            }
            index = xOption;
            if (mainKungFuPoints[index].checkOwner(owner))
            {
                return mainKungFuPoints[index].transform;
            }
            index = (zOption + 2) % 4;
            if (mainKungFuPoints[index].checkOwner(owner))
            {
                return mainKungFuPoints[index].transform;
            }
            index = (xOption + 2) % 4;
            if (mainKungFuPoints[index].checkOwner(owner))
            {
                return mainKungFuPoints[index].transform;
            }
        }
        else
        {
            index = xOption;
            if (mainKungFuPoints[index].checkOwner(owner))
            {
                return mainKungFuPoints[index].transform;
            }
            index = zOption;
            if (mainKungFuPoints[index].checkOwner(owner))
            {
                return mainKungFuPoints[index].transform;
            }
            index = (xOption + 2) % 4;
            if (mainKungFuPoints[index].checkOwner(owner))
            {
                return mainKungFuPoints[index].transform;
            }
            index = (zOption + 2) % 4;
            if (mainKungFuPoints[index].checkOwner(owner))
            {
                return mainKungFuPoints[index].transform;
            }
        }
        return null;
    }
    public void DebugPositions()
    {
        Debug.Log("Start Debug KungFuPoints");
        for (int i = 0;i < mainKungFuPoints.Length; i++)
        {
            Debug.Log(mainKungFuPoints[i].Owner);
        }
        Debug.Log("End Debug KungFuPoints");

    }
    ClassMutex<EnemyAI> IEnemyManager.attackingEnemy() => attackingEnemy;

    void clearKungFuPoints()
    {
        for(int i = 0; i < mainKungFuPoints.Length; i++)
        {
            mainKungFuPoints[i].Owner = null;
        }
    }

 //   ClassMutex<EnemyAI> IEnemyManager.enemyClassMutex() => enemyClassMutex;
}

public interface IEnemyManager:IService
{
   // ClassMutex<EnemyAI> enemyClassMutex();
    ClassMutex<EnemyAI> attackingEnemy();
    public Transform getPoint(int index, Enemy owner);
    public bool returnPoint(int index, Enemy owner);
    public Transform getClosestPoint(Enemy owner, out int index);
    public void DebugPositions();

}
public class ClassMutex<T> where T : Object, IMutex
{
    T Owner;
    List<T> queue;
  public  ClassMutex()
    {
        Owner = null;
        queue = new List<T> { };
    }
    public bool getPermission(T offerer,bool priority)
    {
        
        if (Owner == null || Owner.Equals(offerer))
        {

            Owner = offerer;
            return true;
        }
        else
        {
            if (!queue.Contains(offerer)){
                if(priority)
                {
                    queue.Insert(0, offerer);
                }
                else
                {
                    queue.Add(offerer);
                }
            }

            Debug.Log($"Permiso de ataque no concedido a {offerer}, el permiso lo tiene {Owner}");
            return false;
        }
    }
    public bool returnPermission(T returner)
    {
       // Debug.Log(Owner);
       // Debug.Log(Owner == null);

        if (Owner == null || !Owner.Equals(returner))
        {
            if (queue.Contains(returner))
            {
                queue.Remove(returner);
            }
            Debug.Log($"{returner} cantremovepermission  it belongs to {Owner}");
            return false;
        }
        else
        {
            if (queue.Count > 0)
            {
                T newowner = queue[0];
                queue.RemoveAt(0);
                Owner = newowner;
                newowner.givePriority();
            }
            else
            {
                Owner = null;
            }
                return true;
        }
    }
    public void printOwner()
    {
        Debug.Log(Owner.name);
    }
    public void clearMutex()
    {
        Owner = null;
        queue.Clear();
    }
}
public interface IMutex
{
    public void givePriority();
}