using System;
using UnityEngine;
using Services;
public interface IGameStateManager : IService
{
    public GameState getState();
    public void Pause();
    public void UnPause();
    public void slowDown();
    public void slowDownOff();

    public void Die();
    public void Restart();
    public void startCutscene();

    public void startCombat();

   // public void endCutscene();


    public void subscribeToStateChange(EventHandler<stateData> response);
    public void unSubscribeToStateChange(EventHandler<stateData> response);
    public void subscribeToRestart(Action response);

    public void unSubscribeToRestart(Action response);
}
public enum GameState
{
    Playing,
    Paused,
    Cutscene,
    Dialog,
    SlowDown,
    DeathScreen
}
