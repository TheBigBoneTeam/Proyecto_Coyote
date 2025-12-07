using Services;
using tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tutorialLevelManager : MonoBehaviour, ILevelManager
{
    [SerializeField] CutsceneData cutsceneData;
    ditherTransition dither;
    [SerializeField] bool playCutscene;
    public void Instantiate()
    {

    }

    public void loadEscene(string sceneName)
    {
        dither.goIn(sceneName);
    }
    void Start()
    {
        dither = FindAnyObjectByType<ditherTransition>();
    }
    public void trueStart()
    {
        if (playCutscene && cutsceneData != null)
        {
            ServiceLocator.Instance.Get<IcutsceneManager>().startCutscene(cutsceneData.cutscene, () => { startTutorial(); }, cutsceneData);

        }
        else
        {
            startTutorial();
        }
        ServiceLocator.Instance.Get<ISaveManager>().saveGame(SceneManager.GetActiveScene().name);
    }
    public void startTutorial(){
        ServiceLocator.Instance.Get<IGameStateManager>().Restart(); 

        ServiceLocator.Instance.Get<IGameStateManager>().startCombatforTutorial();
        print("startTut");
        FindAnyObjectByType<TutorialDefenseAttackUIIndicator>().restartTut();
        FindAnyObjectByType<Tutorial>().startTutorial();

    }
}