using UnityEngine;
using Services;


public class PauseMenu : MonoBehaviour
{
    IGameStateManager gameStateManager;
    [SerializeField] Canvas pauseMenuCanvas;
    void Start()
    {
        gameStateManager = ServiceLocator.Instance.Get<IGameStateManager>();
        pauseMenuCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Input System
        if (Input.GetKeyDown(KeyCode.Escape))
            OpenMenu();
    }

    public void OpenMenu() 
    { 
        gameStateManager.Pause();
        GameState state = gameStateManager.getState();
        if (state.Equals(GameState.Paused))
        {
            pauseMenuCanvas.gameObject.SetActive(true);
            Cursor.visible = true;
            
        }
        Debug.Log("Estado: "+ state);
    }
    

    public void ResumeGame()
    {
        gameStateManager.UnPause();
        pauseMenuCanvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }
    public void Reiniciar()
    {
        // Reiniciar
    }
    public void ExitGame()
    {
        // Menu de inicio
    }
}
