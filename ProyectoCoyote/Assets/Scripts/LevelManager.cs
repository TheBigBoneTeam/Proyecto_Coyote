using Services;
using UnityEngine;

public class LevelManager : MonoBehaviour,ILevelManager
{
    CutsceneData cutsceneData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  public void trueStart()
    {
        ServiceLocator.Instance.Get<IcutsceneManager>().startCutscene(cutsceneData.cutscene, () => { ServiceLocator.Instance.Get<IGameStateManager>().startNonCombatGameplay(); }, cutsceneData);
    }

    public void Instantiate()
    {

    }
}
public interface ILevelManager : IService
{
   public void trueStart();
}