using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuSceneChanger : MonoBehaviour
{
    public string primerNivel = "cinematicaIntro";
   [SerializeField] Button continueButton;
    [SerializeField] MenuAnimHandler menuAnimHandler;
    public string continueLvl;
    public void changeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void newGame()
    {
        menuAnimHandler.isNewGame = true;
        menuAnimHandler.StartWalk();
        //SceneManager.LoadScene(primerNivel);
    }
    public void continueGame()
    {
        menuAnimHandler.isNewGame = false;
        menuAnimHandler.StartWalk();
        //SceneManager.LoadScene(continueLvl);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveManager saveManager = new SaveManager();
       string scene = saveManager.getSavedScene();
        if(scene != null)
        {
            continueButton.interactable = true;
            continueLvl = scene;
        }
        else
        {
            continueButton.interactable = false;
            continueLvl = primerNivel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
