using System;
using UnityEngine;
using Services;
public interface IGameStateManager : IService
{
    public GameState getState();
    public void PauseUnpause();
    public void UnPause();
    public void slowDown();
    public void slowDownOff(bool timeout = false);

    public void Die();
    public void Restart();
    public void startCutscene();

    public void startDialog();

    public void startNonCombatGameplay();
    public void startCombat(combatAreaManager combatArea,WaveData waveData);


    public void startCombatforTutorial();


    // public void endCutscene();


    public void subscribeToStateChange(EventHandler<stateData> response);
    public void unSubscribeToStateChange(EventHandler<stateData> response);
    public void subscribeToRestart(Action response);

    public void unSubscribeToRestart(Action response);

    public void subscribeCombatAreaChange(Action<combatAreaManager, WaveData> response);
    public void unSubscribeCombatAreaChange(Action<combatAreaManager, WaveData> response);
}
public enum GameState
{
    NonCombat,
    Paused,
    Cutscene,
    Dialog,
    SlowDown,
    DeathScreen,
    Combat
}
