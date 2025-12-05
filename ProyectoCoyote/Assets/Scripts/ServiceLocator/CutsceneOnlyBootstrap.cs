
using UnityEngine;
namespace Services
{
    public class CutsceneOnlyBootstrap : MonoBehaviour, IServiceBootstrap
    {
        public void Bootstrap()
        {
            ServiceLocator.Instance.Register<IGameStateManager>(FindFirstObjectByType<GameStateManager>());
            ServiceLocator.Instance.Register<IcutsceneManager>(FindFirstObjectByType<timelineDirector>());
            ServiceLocator.Instance.Register<ISaveManager>(new SaveManager());
            ServiceLocator.Instance.Register<ILevelManager>(FindFirstObjectByType<cutsceneOnlyLevelManager>());


        }
    }
}

