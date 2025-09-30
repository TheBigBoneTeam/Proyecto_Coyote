
using UnityEngine;
namespace Services
{

    public class BaseBootstrap : MonoBehaviour, IServiceBootstrap
    {

        public void Bootstrap()
        {
            ServiceLocator.Instance.Register<IGameStateManager>(FindFirstObjectByType<GameStateManager>());
            ServiceLocator.Instance.Register<IHitStop>(FindFirstObjectByType<HitStopManager>());
        }
    }
}

