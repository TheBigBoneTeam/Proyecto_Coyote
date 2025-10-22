using Services;
using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   [SerializeField] TMP_Text lifeText;

    [SerializeField] CanvasGroup slowDownGroup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindAnyObjectByType<Player>().subscribeToLifeChange(changePlayerLife);
         ServiceLocator.Instance.Get<IGameStateManager>().subscribeToStateChange(StateChange);
        
    }

    private void StateChange(object sender, stateData data)
    {
        switch (data.currentState)
        {
            case GameState.Playing:
                slowDownGroup.alpha = 0;

                break;
            case GameState.Paused:
                break;
            case GameState.Cinematic:
                break;
            case GameState.Dialog:
                break;
            case GameState.SlowDown:
                slowDownGroup.alpha = 1;
                break;
            case GameState.DeathScreen:
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void changePlayerLife(int playerLife)
    {
        lifeText.text = playerLife.ToString();
    }
}
