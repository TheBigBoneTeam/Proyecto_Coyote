using Services;
using System.Collections;
using UnityEngine;

public class PerfectDodgeManager:MonoBehaviour,IPerfectDodgeManager
{
  [SerializeField]  float slowdownFactor = 0.6f;
  [SerializeField]  float slowdownDuration = 0.6f;

    private bool slowOn;

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
    }
    public void StopSlowdown()
    {
        if (slowOn)
        {
            Time.timeScale = 1;
            slowOn = false;
        }
    }
    public void StartSlowdown()
    {
        if (!slowOn)
        {
            Time.timeScale = slowdownFactor;
            StartCoroutine(restartTime(slowdownDuration));
            gameStateManager.slowDown();
            slowOn = true;
        }
    }

    IEnumerator restartTime(float SlowDuration)
    {
        for (int i = 0; i < 10; i++)
        {
            if (!slowOn)
            {
                break;
            }
            yield return new WaitForSeconds(SlowDuration / 10);
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