using Services;
using UnityEngine;

public class EnemyManager: IEnemyManager
{
    ClassMutex<EnemyAI> enemyClassMutex;
    ClassMutex<EnemyAI> attackingEnemy;

    public void Instantiate()
    {
        enemyClassMutex = new ClassMutex<EnemyAI>();
        attackingEnemy = new ClassMutex<EnemyAI>();
    }

    ClassMutex<EnemyAI> IEnemyManager.attackingEnemy() => attackingEnemy;

    ClassMutex<EnemyAI> IEnemyManager.enemyClassMutex() => enemyClassMutex;
}
public interface IEnemyManager:IService
{
    ClassMutex<EnemyAI> enemyClassMutex();
    ClassMutex<EnemyAI> attackingEnemy();
}
public class ClassMutex<T> where T : Object
{
    T Owner;

  public  ClassMutex()
    {
        Owner = null;
    }
    public bool getPermission(T offerer)
    {
        
        if (Owner == null || Owner.Equals(offerer))
        {

            Owner = offerer;
            return true;
        }
        else
        {
            Debug.Log($"Permiso de ataque no concedido, el permiso lo tiene {Owner}");
            return false;
        }
    }
    public bool returnPermission(T returner)
    {
        Debug.Log(Owner);
        Debug.Log(Owner == null);

        if (Owner == null || !Owner.Equals(returner))
        {
            return false;
        }
        else
        {
            Owner = default;

            return true;
        }
    }
}
