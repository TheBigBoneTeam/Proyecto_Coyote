using BehaviourAPI.UnityToolkit;
using Services;
using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour, IGameStateManager
{
    //El delegado para avisar de cambios de estado
    public event EventHandler<stateData> onStateChange;
    public Action<combatAreaManager,WaveData> combatAreaChange;
    public Action restartArea;
    //Guarda el estado antes de pausar, para que se pueda pausar en gameplay, cinematicas y tal
    public GameState prePauseState;
    public GameState getState() => currentState;

    //Estado actual (privado)
  [SerializeField]  private GameState currentState;

    //Si se puede pausar o no (por ahora no se usa pero quien sabe)
    private bool canPause;

    combatAreaManager currentCombatArea;
    private WaveData currentWaveData;

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
    
    public void PauseUnpause()
    {
        if (/*canPause &&*/ currentState != GameState.Paused && currentState != GameState.DeathScreen)
        {
            Pause();
        }
        else
        {
            UnPause();
        }
    }
    void Pause()
    {
        if (/*canPause &&*/ currentState != GameState.Paused)
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
        if (currentState == GameState.Combat)
        {
            SetState(GameState.SlowDown);
            // SFX SlowDown
            AudioManager.Instance.PlaySimpleSound("SFX - SlowDown", false, Vector2.zero, true, true);
        }
    }

    public void slowDownOff()
    {
        print("slowdownoff");
        if (currentState == GameState.SlowDown)
        {
            print("slowdownoffconfirmed");

            SetState(GameState.Combat);
        }
    }

    public void Die()
    {
        if(currentState != GameState.DeathScreen)
        {
            currentCombatArea = null;
            currentWaveData = null;
            SetState(GameState.DeathScreen);
        }

        // AudioManager.Instance.PlaySimpleSound("OST - Derrota", false, Vector2.zero, true, true);
    }

    public void Restart()
    {
        if (currentState == GameState.DeathScreen)
        {
            restartArea?.Invoke();
            SetState(GameState.NonCombat);
        }
    }

    public void startCutscene()
    {
        if(currentState!= GameState.Cutscene)
        {
            SetState(GameState.Cutscene);
        }
    }

    public void startCombat(combatAreaManager combatArea, WaveData waveData)
    {
        if (currentState != GameState.Combat && currentState != GameState.SlowDown)
        {
            SetState(GameState.Combat);
        }
        if((combatArea != null && combatArea != currentCombatArea) ||(currentWaveData != null && currentWaveData != waveData))
        {
            print("invokeCombatAreaChange");
            currentCombatArea = combatArea;
            currentWaveData = waveData;
            combatAreaChange?.Invoke(currentCombatArea, currentWaveData);
        }
        else
        {
            print("invalidAreaChange");

        }
    }

    public void startDialog()
    {
        if (currentState == GameState.NonCombat)
        {
            SetState(GameState.Dialog);
        }
    }

    public void endDialog()
    {
        if (currentState == GameState.Dialog)
        {
            SetState(GameState.NonCombat);
        }
    }
    public void startNonCombatGameplay()
    {
        if (currentState != GameState.NonCombat)
        {
            SetState(GameState.NonCombat);
        }
    }

    public void subscribeCombatAreaChange(Action<combatAreaManager, WaveData> response)
    {
        combatAreaChange += response;
    }

    public void unSubscribeCombatAreaChange(Action<combatAreaManager, WaveData> response)
    {
        combatAreaChange -= response;
    }

    public void startCombatforTutorial()
    {
        if (currentState != GameState.Combat && currentState != GameState.SlowDown)
        {
            SetState(GameState.Combat);
        }
    }

   
}
