using System;
using UnityEngine;

public class Gancho : MonoBehaviour
{
    Animator cinemachineAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cinemachineAnimator = GameObject.Find("State-Driven Camera").GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) selectTargetHook();
        
    }

    public void selectTargetHook()
    {
        cinemachineAnimator.Play("Hook_Camera");
        Debug.Log("----------Cámara gancho Activada");
    }
}
