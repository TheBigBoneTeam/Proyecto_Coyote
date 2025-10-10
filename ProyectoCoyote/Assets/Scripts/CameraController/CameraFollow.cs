using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector2 clampAxis = new Vector2 (60, 60);

    [SerializeField] float follow_smoothing = 5.0f;
    [SerializeField] float rotate_smoothing = 5.0f;
    [SerializeField] float sensitivity = 60;

    float rotx, roty;
    bool cursorLocked = false;
    Transform cam;

    public bool lockedTarget;

    void Start()
    {
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main.transform;
    }

    void Update()
    {
        Vector3 target_P = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, target_P, follow_smoothing*Time.deltaTime);

        if (!lockedTarget) CameraTargetRotation(); else LookAtTarget();

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
    }

    void CameraTargetRotation()
    {
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        rotx += (mouseAxis.x * sensitivity) * Time.deltaTime;
        roty += (mouseAxis.y * sensitivity) * Time.deltaTime;

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
