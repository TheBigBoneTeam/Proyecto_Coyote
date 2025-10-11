using System;
using UnityEngine;

// Clase define cómo se comporta la cámara con respecto al objetivo (el jugador)
// La cámara va a seguir al jugador en todo momento, pero en función de si está
// lockeada o no rotará libremente o alrededor del enemigo
public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector2 clampAxis = new Vector2 (60, 60);

    [SerializeField] float follow_smoothing = 5.0f;
    [SerializeField] float rotate_smoothing = 5.0f;
    [SerializeField] float sensitivity = 60;

    float rotx, roty;
    bool cursorLocked = false; //// Input System
    Transform cam;

    public bool lockedTarget;

    void Start()
    {
        //// Input System
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
        ////
        cam = Camera.main.transform;
    }

    void Update()
    {
        // Definir posición del objetivo y transformar su posición en función de éste
        Vector3 target_P = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, target_P, follow_smoothing*Time.deltaTime);

        // Condición de si está lockeado o no
        if (!lockedTarget) CameraTargetRotation(); else LookAtTarget();

        //// Input System
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (cursorLocked)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else 
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        ////
    }
    
    ////Input System
    Vector2 InputCamera()
    {
        Vector2 axis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        return axis;
    }
    ////
    
    void CameraTargetRotation()
    {
        Vector2 axis = InputCamera();
        rotx += (axis.x * sensitivity) * Time.deltaTime;
        roty += (axis.y * sensitivity) * Time.deltaTime;

        roty = Mathf.Clamp(roty, clampAxis.x, clampAxis.y);

        Quaternion localRotation = Quaternion.Euler(roty, rotx, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, localRotation, Time.deltaTime * rotate_smoothing);

    }
    
    void LookAtTarget()
    {
        transform.rotation = cam.rotation;
        Vector3 r = cam.eulerAngles;
        rotx = r.y;
        roty = 1.8f;
    }
}
