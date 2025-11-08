
using UnityEngine;
namespace Services
{

    public class BaseBootstrap : MonoBehaviour, IServiceBootstrap
    {

        public void Bootstrap()
        {
            ServiceLocator.Instance.Register<IGameStateManager>(FindFirstObjectByType<GameStateManager>());
            ServiceLocator.Instance.Register<IHitStop>(FindFirstObjectByType<HitStopManager>());
            ServiceLocator.Instance.Register<IPerfectDodgeManager>(FindFirstObjectByType<PerfectDodgeManager>());
            ServiceLocator.Instance.Register<IEnemyManager>(new EnemyManager());

        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.P))
            {
                ServiceLocator.Instance.Get<IEnemyManager>().attackingEnemy().printOwner();
            }
        }
    }
}

