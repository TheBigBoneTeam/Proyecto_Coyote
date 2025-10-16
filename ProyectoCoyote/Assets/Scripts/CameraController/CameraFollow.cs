using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;

// Clase define cómo se comporta la cámara con respecto al objetivo (el jugador)
// La cámara va a seguir al jugador en todo momento, pero en función de si está
// lockeada o no rotará libremente o alrededor del enemigo
public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public Transform enemy;
    [SerializeField] Vector2 clampAxis = new Vector2(60, 60);
    public bool lockedTarget = false;

    public float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {

        if (!lockedTarget) RotatePlayerUnlocked();
        else RotatePlayerLocked();
        
    }
    public void RotatePlayerUnlocked()
    {
        // Rotar orientacion
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized; 

        // Rotar personaje
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        verticalInput = Mathf.Clamp(verticalInput, clampAxis.x, clampAxis.y);
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, viewDir.normalized, Time.deltaTime * rotationSpeed);
        }

    }
    public void RotatePlayerLocked()
    {
        Debug.Log("Estas lockeado joput");
    }
}
