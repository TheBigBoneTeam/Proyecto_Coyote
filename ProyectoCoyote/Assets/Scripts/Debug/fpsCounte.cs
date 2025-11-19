using UnityEngine;

public class fpsCounte : MonoBehaviour
{
    [SerializeField] int fontSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnGUI()
    {
        GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = fontSize;

        GUI.Label(new Rect(Screen.width -100 , 0, 100, 100), ((int)(1.0f / Time.smoothDeltaTime)).ToString());
    }
}
