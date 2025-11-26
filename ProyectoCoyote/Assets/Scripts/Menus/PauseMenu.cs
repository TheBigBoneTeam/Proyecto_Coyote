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
    void Start()
    {
        gameStateManager = ServiceLocator.Instance.Get<IGameStateManager>();
        pauseMenuCanvas.gameObject.SetActive(false);
        anim = GetComponent<Animator>();

        MenuSceneChanger = new menuSceneChanger();
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
        gameStateManager.PauseUnpause();
        GameState state = gameStateManager.getState();
        if (state.Equals(GameState.Paused))
        {
            pauseMenuCanvas.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        Debug.Log("Estado: "+ state);
    }
    

    public void ResumeGame()
    {
        gameStateManager.UnPause();
        pauseMenuCanvas.gameObject.SetActive(false);
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;

    }
    public void Reiniciar()
    {
        gameStateManager.UnPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        pauseMenuCanvas.gameObject.SetActive(false);
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
}
