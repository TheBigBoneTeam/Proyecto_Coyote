using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCam : MonoBehaviour
{
    public float sensX, sensY;
    public Transform orientation;
    float xRotation, yRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        // Limites de la camara en el eje Y 
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotar la camara en ambos ejes
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // Rotar el personaje en el eje Y
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
