using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cutsceneOnlyLevelManager : ILevelManager
{
    [SerializeField] CutsceneData cutsceneData;
    ditherTransition dither;
    [SerializeField] bool playCutscene;
    [SerializeField] string sceneName;
    public void Instantiate()
    {
       
    }

    public void loadEscene(string sceneName)
    {
        dither.goIn(sceneName);
    }

    public void trueStart()
    {
        if (playCutscene && cutsceneData != null)
        {
            ServiceLocator.Instance.Get<IcutsceneManager>().startCutscene(cutsceneData.cutscene, () => {dither.goIn(sceneName); },cutsceneData);

        }
        else
        {
            dither.goIn(sceneName);
        }
        ServiceLocator.Instance.Get<ISaveManager>().saveGame(SceneManager.GetActiveScene().name);
    }
}