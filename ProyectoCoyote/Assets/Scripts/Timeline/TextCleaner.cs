using TMPro;
using UnityEngine;

public class TextCleaner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponentInChildren<TMP_Text>(true).text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
