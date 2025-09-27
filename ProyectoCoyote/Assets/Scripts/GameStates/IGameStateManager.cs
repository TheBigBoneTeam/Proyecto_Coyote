using System;
using UnityEngine;

public interface IGameStateManager : IService
{
    public GameState getState();
    public void Pause();
    public void UnPause();
    public void subscribeToStateChange(EventHandler<stateData> response);
    public void unSubscribeToStateChange(EventHandler<stateData> response);
}
public enum GameState
{
    Playing,
    Paused,
    Cinematic,
    Dialog
}
