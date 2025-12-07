using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuSceneChanger : MonoBehaviour
{
    [SerializeField] string primerNivel = "cinematicaIntro";
   [SerializeField] Button continueButton;
    string continueLvl;
    public void changeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void newGame()
    {
        SceneManager.LoadScene(primerNivel);
    }
    public void continueGame()
    {
        SceneManager.LoadScene(continueLvl);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveManager saveManager = new SaveManager();
       string scene = saveManager.getSavedScene();
        if(scene != null)
        {
            continueButton.enabled = true;
            continueLvl = scene;
        }
        else
        {
            continueButton.enabled = false;
            continueLvl = primerNivel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
