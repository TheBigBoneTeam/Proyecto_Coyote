using UnityEngine;
using Services;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    IGameStateManager gameStateManager;
    menuSceneChanger MenuSceneChanger;

    Animator anim;
    [SerializeField] Canvas pauseMenuCanvas;
    string scene;
    GameInput gameInput;
    void Start()
    {
        gameInput = FindAnyObjectByType<GameInput>();
        gameStateManager = ServiceLocator.Instance.Get<IGameStateManager>();
        pauseMenuCanvas.gameObject.SetActive(false);
        anim = GetComponent<Animator>();

        MenuSceneChanger = new menuSceneChanger();
    }

    // Update is called once per frame
    void Update()
    {
        // Input System
        if (gameInput.EscapePressed)
            OpenMenu();
    }

    public void OpenMenu() 
    { 
        gameStateManager.PauseUnpause();
        GameState state = gameStateManager.getState();
        if (state.Equals(GameState.Paused) )
        {
            pauseMenuCanvas.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            pauseMenuCanvas.gameObject.SetActive(false);
            if (!state.Equals(GameState.DeathScreen))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
            Debug.Log("Estado: " + state);
    }
    

    public void ResumeGame()
    {
        gameStateManager.PauseUnpause();
        pauseMenuCanvas.gameObject.SetActive(false);
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;

    }
    public void ReiniciarArea()
    {
        gameStateManager.PauseUnpause();
        gameStateManager.Restart();
        pauseMenuCanvas.gameObject.SetActive(false);
    }
    public void Reiniciar()
    {
        gameStateManager.PauseUnpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        pauseMenuCanvas.gameObject.SetActive(false);
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ExitGame()
    {
        gameStateManager.PauseUnpause();
        SceneManager.LoadScene(0);
    }
}
