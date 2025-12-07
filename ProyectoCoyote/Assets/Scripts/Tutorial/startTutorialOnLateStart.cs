using System.Collections;
using tutorial;
using Unity.VisualScripting;
using UnityEngine;

public class startTutorialOnLateStart : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     //   StartCoroutine(waitEnd());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator waitEnd()
    {
        yield return new WaitForNextFrameUnit();
        GetComponent<Tutorial>().startTutorial();

    }
}
