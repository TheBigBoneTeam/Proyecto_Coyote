using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour, IGameStateManager
{
    //El delegado para avisar de cambios de estado
    public event EventHandler<stateData> onStateChange;

    //Guarda el estado antes de pausar, para que se pueda pausar en gameplay, cinematicas y tal
    public GameState prePauseState;
    public GameState getState() => currentState;

    //Estado actual (privado)
    private GameState currentState;

    //Si se puede pausar o no (por ahora no se usa pero quien sabe)
    private bool canPause;
    public void Instantiate()
    {
    }
    //Cambia el estado directamente (privado)
    private void SetState(GameState state)
    {
        stateData stData = new stateData(currentState, state);
        onStateChange.Invoke(this, stData);
        currentState = state;

    }
    public void Pause()
    {
        if (canPause && currentState != GameState.Paused)
        {
            prePauseState = currentState;
            SetState(GameState.Paused);
        }
    }
    public void UnPause()
    {

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
}
