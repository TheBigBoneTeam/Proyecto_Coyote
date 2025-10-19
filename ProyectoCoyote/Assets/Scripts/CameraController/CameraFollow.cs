using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTarget; 
    Transform player;
    Transform playerObj;
    EnemyLockOn enemyLockOn;

    [Header("Settings")]
    public float rotationSpeed = 5f;

    private bool lockedCamera;

    private void Start()
    {
        if (!player) player = GameObject.Find("Player").transform;
        if (!playerObj) playerObj = GameObject.Find("Player/Player_02").transform;
        if (!enemyLockOn) enemyLockOn = GameObject.FindAnyObjectByType<EnemyLockOn>();
    }

    private void LateUpdate()
    {
        lockedCamera = enemyLockOn != null && enemyLockOn.enemyLocked;

        if (lockedCamera) RotateLockedPlayer();
        else RotateFreePlayer();
    }

    private void RotateFreePlayer()
    {
        // Dirección desde cámara hacia jugador
        Vector3 viewDir = player.position - new Vector3(Camera.main.transform.position.x, player.position.y, Camera.main.transform.position.z);
        Vector3 forward = viewDir.normalized;

        // Input del jugador
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = forward * verticalInput + Camera.main.transform.right * horizontalInput;

        // Rotar el jugador si hay input
        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, forward, Time.deltaTime * rotationSpeed);
        }

        // Actualizar posición del target de cámara
        cameraTarget.position = player.position + Vector3.up * 2f;
    }

    private void RotateLockedPlayer()
    {
        if (enemyLockOn.currentTarget == null) return;

        Vector3 enemyPos = enemyLockOn.currentTarget.position;

        // Posición detrás jugador
        Vector3 backDir = -player.forward;
        Vector3 desiredPosition = player.position + backDir  + Vector3.up;

        // Mover el target de cámara
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, desiredPosition, Time.deltaTime * rotationSpeed);

        // Rotar el target hacia el enemigo
        Vector3 lookDir = (enemyPos + Vector3.up * 1.5f) - cameraTarget.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDir);
        cameraTarget.rotation = Quaternion.Slerp(cameraTarget.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Rotar el jugador hacia el enemigo
        Vector3 viewDir = enemyPos - player.position;
        viewDir.y = 0;
        if (viewDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, viewDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
