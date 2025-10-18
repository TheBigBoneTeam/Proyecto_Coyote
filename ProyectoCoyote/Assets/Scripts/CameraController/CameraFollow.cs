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
    EnemyLockOn enemyLockOn;

    public float rotationSpeed;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        orientation = GameObject.Find("Player/Orientation").transform;
        playerObj = GameObject.Find("Player/Player_01").transform;
        enemyLockOn = GameObject.FindAnyObjectByType<EnemyLockOn>(); 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        LockedCamera = enemyLockOn.enemyLocked;

        
       if (LockedCamera ) RotatePlayer(); else RotatePlayer();

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
        // Rotar orientacion
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        playerObj.forward = Vector3.Slerp(playerObj.forward, viewDir.normalized, Time.deltaTime * rotationSpeed);
        
    }

    //[SerializeField] Transform target;
    //[SerializeField] Vector3 offset;
    //[SerializeField] Vector2 clampAxis = new Vector2 (60, 60);

    //[SerializeField] float follow_smoothing = 5.0f;
    //[SerializeField] float rotate_smoothing = 5.0f;
    //[SerializeField] float sensitivity = 60;

    //float rotx, roty;
    //bool cursorLocked = false; //// Input System
    //Transform cam;

    //public bool lockedTarget;

    //void Start()
    //{
    //    //// Input System
    //    Cursor.visible = false; 
    //    Cursor.lockState = CursorLockMode.Locked;
    //    ////
    //    cam = Camera.main.transform;
    //}

    //void Update()
    //{
    //    // Definir posición del objetivo y transformar su posición en función de éste
    //    Vector3 target_P = target.position + offset;
    //    transform.position = Vector3.Lerp(transform.position, target_P, follow_smoothing*Time.deltaTime);

    //    // Condición de si está lockeado o no
    //    if (!lockedTarget) CameraTargetRotation(); else LookAtTarget();

    //    //// Input System
    //    if (Input.GetKeyDown(KeyCode.Escape)) 
    //    {
    //        if (cursorLocked)
    //        {
    //            Cursor.visible = true;
    //            Cursor.lockState = CursorLockMode.None;
    //        }
    //        else 
    //        {
    //            Cursor.visible = false;
    //            Cursor.lockState = CursorLockMode.Locked;
    //        }
    //    }
    //    ////
    //}

    //////Input System
    //Vector2 InputCamera()
    //{
    //    Vector2 axis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    //    return axis;
    //}
    //////

    //void CameraTargetRotation()
    //{
    //    Vector2 axis = InputCamera();
    //    rotx += (axis.x * sensitivity) * Time.deltaTime;
    //    roty += (axis.y * sensitivity) * Time.deltaTime;

    //    roty = Mathf.Clamp(roty, clampAxis.x, clampAxis.y);

    //    Quaternion camRotation = Quaternion.Euler(roty, rotx, 0);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, camRotation, Time.deltaTime * rotate_smoothing);

    //    // Hacer que el jugador mire hacia donde apunta la cámara 
    //    if (!lockedTarget && target != null)
    //    {
    //        Vector3 lookDir = transform.forward;
    //        lookDir.y = 0;
    //        if (lookDir != Vector3.zero)
    //        {
    //            Quaternion playerRot = Quaternion.LookRotation(lookDir);
    //            target.rotation = Quaternion.Slerp(target.rotation, playerRot, Time.deltaTime * rotate_smoothing);
    //        }
    //    }

    //}

    //void LookAtTarget()
    //{
    //    Quaternion targetRotation = Quaternion.LookRotation(target.forward, Vector3.up);

    //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotate_smoothing * Time.deltaTime);

    //    Vector3 euler = transform.eulerAngles;
    //    rotx = euler.y;
    //    roty = euler.x;
    //}
}
