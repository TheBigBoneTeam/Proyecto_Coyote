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
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeToRestart(()=>attackingEnemy.clearMutex());
      
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
        if (mainKungFuPoints[index].checkOwner(owner))
        {
            mainKungFuPoints[index].Owner = null;
            return true;
        }
        Debug.Log("isNull");
        EnemyAssetBehaviourRunner ase;
      
        return false;

    }

    //public Transform getClosestPoint(Enemy owner, out int index)
    //{
    //    Vector3 playerPos = player.transform.position;
    //    Vector3 enemyPos = owner.transform.position;

    //    int 
    //    if(playerPos.z > enemyPos.z)
    //    {

    //    }
    //}

    ClassMutex<EnemyAI> IEnemyManager.attackingEnemy() => attackingEnemy;

 //   ClassMutex<EnemyAI> IEnemyManager.enemyClassMutex() => enemyClassMutex;
}

public interface IEnemyManager:IService
{
   // ClassMutex<EnemyAI> enemyClassMutex();
    ClassMutex<EnemyAI> attackingEnemy();
    public Transform getPoint(int index, Enemy owner);



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

            Debug.Log($"Permiso de ataque no concedido, el permiso lo tiene {Owner}");
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
            return false;
        }
        else
        {
            if (queue.Count > 0)
            {
                T newowner = queue[0];
                queue.RemoveAt(0);
                Owner = newowner;
                Owner.givePriority();
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