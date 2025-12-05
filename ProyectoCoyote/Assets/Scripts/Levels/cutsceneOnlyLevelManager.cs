using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cutsceneOnlyLevelManager :MonoBehaviour, ILevelManager
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
            ServiceLocator.Instance.Get<IcutsceneManager>().startCutscene(cutsceneData.cutscene, () => {dither.goIn(cutsceneData.nextLevel); },cutsceneData);

        }
        else
        {
            dither.goIn(cutsceneData.nextLevel);
        }
        ServiceLocator.Instance.Get<ISaveManager>().saveGame(SceneManager.GetActiveScene().name);
    }
}