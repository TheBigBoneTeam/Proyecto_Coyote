using Services;
using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   [SerializeField] TMP_Text lifeText;

    [SerializeField] CanvasGroup slowDownGroup;
    [SerializeField] CanvasGroup lifeGroup;

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
            case GameState.Combat:
                slowDownGroup.alpha = 0;
                lifeGroup.alpha = 1;

                break;
            case GameState.Paused:
                lifeGroup.alpha = 0;

                break;
            case GameState.Cutscene:
                lifeGroup.alpha = 0;

                break;
            case GameState.Dialog:
                lifeGroup.alpha = 0;

                break;
            case GameState.SlowDown:
                slowDownGroup.alpha = 1;
                lifeGroup.alpha = 1;

                break;
            case GameState.DeathScreen:
                lifeGroup.alpha = 0;
                break;
            case GameState.NonCombat:
                lifeGroup.alpha = 1;
                slowDownGroup.alpha = 0;

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
