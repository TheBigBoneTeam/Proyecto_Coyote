using Services;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, ILevelManager
{
   [SerializeField] CutsceneData cutsceneData;
    ditherTransition dither;
    [SerializeField] bool playCutscene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dither = FindAnyObjectByType<ditherTransition>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void trueStart()
    {
        if (playCutscene && cutsceneData != null)
        {
            ServiceLocator.Instance.Get<IcutsceneManager>().startCutscene(cutsceneData.cutscene, () => { ServiceLocator.Instance.Get<IGameStateManager>().Restart(); ServiceLocator.Instance.Get<IGameStateManager>().startNonCombatGameplay(); }, cutsceneData);

        }
        else
        {
            ServiceLocator.Instance.Get<IGameStateManager>().Restart();
            ServiceLocator.Instance.Get<IGameStateManager>().startNonCombatGameplay();
        }
        ServiceLocator.Instance.Get<ISaveManager>().saveGame(SceneManager.GetActiveScene().name);
    }

    public void Instantiate()
    {

    }

    public void loadEscene(string sceneName)
    {
        dither.goIn(sceneName);
    }

}
public interface ILevelManager : IService
{
    public void trueStart();
    public void loadEscene(string sceneName);
}
