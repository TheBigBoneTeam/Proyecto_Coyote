using Services;
using System.Collections;
using UnityEngine;

public class PerfectDodgeManager:MonoBehaviour,IPerfectDodgeManager
{
  [SerializeField]  float slowdownFactor = 0.6f;
  [SerializeField]  float slowdownDuration = 0.6f;

    [SerializeField] private bool slowOn;

    IGameStateManager gameStateManager;

    public void subscribetoTimeChange()
    {

    }
    public void Instantiate()
    {
    }
    public void Start()
    {
        gameStateManager = ServiceLocator.Instance.Get<IGameStateManager>();
        gameStateManager.subscribeToStateChange(StateChange);
    }
    public void StateChange(object sender, stateData stateData)
    {
        if(stateData.currentState == GameState.SlowDown)
        {
            slowOn = true;
            Time.timeScale = slowdownFactor;
            StartCoroutine(restartTime(slowdownDuration));
        }
        if (stateData.oldState == GameState.SlowDown)
        {
            Time.timeScale = 1;
            slowOn = false;
        }
    }
    public void StopSlowdown()
    {
        print("stop");
        if (slowOn)
        {
            gameStateManager.slowDownOff();

        }
    }
    public void StartSlowdown()
    {
        if (!slowOn)
        {
            
            gameStateManager.slowDown();
        }
    }

    IEnumerator restartTime(float SlowDuration)
    {
        float usedDuration = SlowDuration;
        for (int i = 0; i < 10; i++)
        {
            if (!slowOn)
            {
                break;
            }
            yield return new WaitForSeconds(usedDuration / 10);
        }
        StopSlowdown();
    }

    public bool isSlowDown() => slowOn;
    public float slowDownFactor() => slowdownFactor;

}
public interface IPerfectDodgeManager : IService
{
    public void StopSlowdown();
    public void StartSlowdown();
    public bool isSlowDown();
    public float slowDownFactor();
}