using Services;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HitStopManager :MonoBehaviour, IHitStop
{
    IGameStateManager _gameStateManager;
    private bool isStopped;
    public void GeneralScreenFreeze(float duration, float delay = 0)
    {
        if (isStopped)
            return;
        Time.timeScale = 0;
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
    public void Start()
    {
        _gameStateManager = ServiceLocator.Instance.Get<IGameStateManager>();
    }
    IEnumerator ScreenFreeze(float duration)
    {
        int x = 5;
        for (int i = 0; i < x; i++)
        {

            yield return new WaitUntil(() => _gameStateManager.getState() == GameState.Playing);
            yield return new WaitForSecondsRealtime(duration / x);
            
        }
        isStopped = false;
        Time.timeScale = 1;
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
}
public interface IHitStop : IService
{
    public void GeneralScreenFreeze(float duration, float delay = 0);
}

public class HitStopComponent : MonoBehaviour
{
    IGameStateManager _gameStateManager;
    private Rigidbody _rigidbody;
    private bool isStopped;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _gameStateManager = ServiceLocator.Instance.Get<IGameStateManager>();
    }
    public void GeneralScreenFreeze(float duration, float delay = 0)
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
    IEnumerator ScreenFreeze(float duration)
    {
        int x = 5;
        for (int i = 0; i < x; i++)
        {

            yield return new WaitUntil(() => _gameStateManager.getState() == GameState.Playing);
            yield return new WaitForSecondsRealtime(duration / x);

        }
        isStopped = false;
        Time.timeScale = 1;
    }
    IEnumerator ScreenFreezeDelay(float duration, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        StartCoroutine(ScreenFreeze(duration));

    }
}
