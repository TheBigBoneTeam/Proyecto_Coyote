using Services;
using UnityEngine;

public class EnemyManager: IEnemyManager
{
   ClassMutex<Enemy> enemyClassMutex;
    ClassMutex<Enemy> attackingEnemy;

    public void Instantiate()
    {
        enemyClassMutex = new ClassMutex<Enemy>();
        attackingEnemy = new ClassMutex<Enemy>();
    }

    ClassMutex<Enemy> IEnemyManager.attackingEnemy() => attackingEnemy;

    ClassMutex<Enemy> IEnemyManager.enemyClassMutex() => enemyClassMutex;
}
public interface IEnemyManager:IService
{
    ClassMutex<Enemy> enemyClassMutex();
    ClassMutex<Enemy> attackingEnemy();
}
public class ClassMutex<T>
{
    T Owner;
    public bool getPermission(T offerer)
    {
        if (Owner.Equals(default(T)) || Owner.Equals(offerer))
        {

            Owner = offerer;
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool returnPermission(T returner)
    {
        if (Owner.Equals(returner))
        {
         Owner =   default(T);
            return true;
        }
        else
        {
            return true;
        }
    }
}
