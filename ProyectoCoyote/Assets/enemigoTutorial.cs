using UnityEngine;

public class enemigoTutorial : MonoBehaviour
{
    [SerializeField] int tutorialMode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setTutorialMode(int mode)
    {
        tutorialMode = mode;
    }
    public int checkTutorialMode()=> tutorialMode;
}
