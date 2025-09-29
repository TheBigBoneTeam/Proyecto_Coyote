using Services;
using System.Collections;
using UnityEngine;

public abstract class AHitstopBase :MonoBehaviour
{
    IGameStateManager _gameStateManager;
    private bool isStopped;
    public void HitStop(float duration, float delay = 0)
    {
        if (isStopped)
            return;
        isStopped = true;
        if (delay == 0)
        {
            StartCoroutine(ScreenFreeze(duration));
        }
        else
        {
            StartCoroutine(ScreenFreezeDelay(duration, delay));
        }
    }
    public virtual void Start()
    {
        _gameStateManager = ServiceLocator.Instance.Get<IGameStateManager>();
    }
    IEnumerator ScreenFreeze(float duration)
    {
        Stop();
        int x = 5;
        for (int i = 0; i < x; i++)
        {

            yield return new WaitUntil(() => _gameStateManager.getState() == GameState.Playing);
            yield return new WaitForSecondsRealtime(duration / x);

        }
        isStopped = false;
        Continue();
    }
    IEnumerator ScreenFreezeDelay(float duration, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        StartCoroutine(ScreenFreeze(duration));

    }
    public void Instantiate()
    {
        isStopped = false;
    }
    protected abstract void Stop();
    protected abstract void Continue();
}
