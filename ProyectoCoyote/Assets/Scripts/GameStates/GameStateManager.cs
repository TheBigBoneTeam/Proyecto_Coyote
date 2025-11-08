using BehaviourAPI.UnityToolkit;
using Services;
using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour, IGameStateManager
{
    //El delegado para avisar de cambios de estado
    public event EventHandler<stateData> onStateChange;
    public Action restartArea;
    //Guarda el estado antes de pausar, para que se pueda pausar en gameplay, cinematicas y tal
    public GameState prePauseState;
    public GameState getState() => currentState;

    //Estado actual (privado)
  [SerializeField]  private GameState currentState;

    //Si se puede pausar o no (por ahora no se usa pero quien sabe)
    private bool canPause;

    IPerfectDodgeManager perfectDodgeManager;
    public void Instantiate()
    {
    }
    //Cambia el estado directamente (privado)
    private void SetState(GameState state)
    {
        stateData stData = new stateData(currentState, state);
        onStateChange?.Invoke(this, stData);
        currentState = state;

    }
    private void Start()
    {
        perfectDodgeManager = ServiceLocator.Instance.Get<IPerfectDodgeManager>(); 
    }
    public void Pause()
    {
        if (canPause && currentState != GameState.Paused)
        {
            prePauseState = currentState;
            Time.timeScale = 0;
            SetState(GameState.Paused);
        }
    }
    public void UnPause()
    {
        if (prePauseState == GameState.SlowDown)
        {
            Time.timeScale = perfectDodgeManager.slowDownFactor();
        }
        else
        {
            Time.timeScale = 1;
        }
        SetState(prePauseState);

    }
    public void subscribeToStateChange(EventHandler<stateData> response)
    {
        onStateChange += response;

    }
    public void unSubscribeToStateChange(EventHandler<stateData> response)
    {
        onStateChange -= response;
    }
    public void subscribeToRestart(Action response)
    {
        restartArea += response;

    }
    public void unSubscribeToRestart(Action response)
    {
        restartArea -= response;
    }

    public void slowDown()
    {
        if (currentState == GameState.Playing)
        {
            SetState(GameState.SlowDown);
        }
    }

    public void slowDownOff()
    {
        print("slowdownoff");
        if (currentState == GameState.SlowDown)
        {
            print("slowdownoffconfirmed");

            SetState(GameState.Playing);
        }
    }

    public void Die()
    {
        if(currentState != GameState.DeathScreen)
        {
            SetState(GameState.DeathScreen);
        }
    }

    public void Restart()
    {
        if (currentState == GameState.DeathScreen)
        {
            restartArea?.Invoke();
            SetState(GameState.Playing);
        }
    }

    public void startCutscene()
    {
        if(currentState!= GameState.Cutscene)
        {
            SetState(GameState.Cutscene);
        }
    }

   
}
