using System;
using UnityEngine;

// Clase define cómo se comporta la cámara con respecto al objetivo (el jugador)
// La cámara va a seguir al jugador en todo momento, pero en función de si está
// lockeada o no rotará libremente o alrededor del enemigo
public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    Transform orientation;
    Transform player;
    Transform playerObj;
    public bool LockedCamera = false;
 [SerializeField]   EnemyLockOn enemyLockOn;

    public float rotationSpeed;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        orientation = GameObject.Find("Player/Orientation").transform;
        playerObj = GameObject.Find("Player/Player_02").transform;
        enemyLockOn = GameObject.FindAnyObjectByType<EnemyLockOn>(); 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        LockedCamera = enemyLockOn.enemyLocked;

        
       if (!LockedCamera ) RotatePlayer(); else RotateLockedPlayer();

    }
    public void RotatePlayer() 
    {
        // Rotar orientacion
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // Rotar personaje
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, viewDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
    public void RotateLockedPlayer()
    {
        if (enemyLockOn.currentTarget == null) return;

        Vector3 enemyPos = enemyLockOn.currentTarget.position;

        // Posición fija detrás del jugador
        float distance = 4f; // distancia detrás del jugador
        float height = 2f;   // altura de la cámara

        // Dirección hacia atrás del jugador
        Vector3 backDir = -player.forward;
        Vector3 desiredPosition = player.position + backDir * distance + Vector3.up * height;

        // Mover la cámara detrás del jugador
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * rotationSpeed);

        // Mirar al enemigo
        Vector3 lookTarget = enemyPos + Vector3.up * 1.5f;
        Vector3 lookDir = lookTarget - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Rotar el jugador hacia el enemigo
        Vector3 viewDir = enemyPos - player.position;
        viewDir.y = 0;
        if (viewDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, viewDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }

}
