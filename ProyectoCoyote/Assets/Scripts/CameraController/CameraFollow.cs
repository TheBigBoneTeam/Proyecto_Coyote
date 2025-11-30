using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public CinemachineCamera lockOnCamera;
    public CinemachineCamera hookCamera;
    public Transform cameraTarget;

    Transform player;
    Transform playerObj;
    EnemyLockOn enemyLockOn;
    Gancho hook;
    HandleOcclusions handleOcclusions;



    [Header("Settings")]
    public float rotationSpeed = 5f;
    public float minDistanceToSwitch = 3f;
    private bool lockedCamera;
    private bool hookedCamera;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        playerObj = GameObject.Find("Player/Player_02").transform;
        enemyLockOn = GameObject.FindAnyObjectByType<EnemyLockOn>();
        hook = GameObject.FindAnyObjectByType<Gancho>();
        handleOcclusions = GameObject.FindAnyObjectByType<HandleOcclusions>();
    }

    private void LateUpdate()
    {
        lockedCamera = enemyLockOn != null && enemyLockOn.enemyLocked;
        hookedCamera = hook != null && hook.selectingHook;

        if (lockedCamera) HandleLockCamera(enemyLockOn.currentTarget, lockOnCamera);
        else if (hookedCamera) HandleTarget(hook.currentTarget, hookCamera, hook.lookAtRotationOffset);
        else RotateFreePlayer();

        handleOcclusions.HandleTransparency();

    }

    private void RotateFreePlayer()
    {
        // Dirección desde cámara hacia jugadorObj
        Vector3 viewDir = playerObj.position - new Vector3(Camera.main.transform.position.x, playerObj.position.y, Camera.main.transform.position.z);
        Vector3 forward = viewDir.normalized;

        // Input del jugador
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = forward * v + Camera.main.transform.right * h;

        // Rotar el jugador si hay input
        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, forward, Time.deltaTime * rotationSpeed);
        }

        // Actualizar posición del target de cámara
        cameraTarget.position = playerObj.position + Vector3.up * 2f;
    }

    private void HandleLockCamera(Transform enemy, CinemachineCamera cam)
    {
        if (enemy == null || cam == null) return;
        
        // Rotar el jugador hacia el enemigo
        Vector3 lookToEnemy = enemy.position - playerObj.position;
        lookToEnemy.y = 0f;
        if (lookToEnemy.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookToEnemy);
            playerObj.rotation = Quaternion.Slerp(playerObj.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }
        cameraTarget.position = playerObj.position + Vector3.up * 2f;

        // Alinea la rotación base del pivot con la espalda del jugador
        cameraTarget.rotation = Quaternion.Euler(0f, playerObj.eulerAngles.y, 0f);

    }

    // Hook
    private void HandleTarget(Transform target, CinemachineCamera cam, Vector3 rotationOffset = default)
    {
        if (target == null || cam == null) return;

        Vector3 targetPos = target.position;
        float distance = Vector3.Distance(playerObj.position, targetPos);

        // Offset
        Vector3 playerOffset = playerObj.position - playerObj.forward * 2f + Vector3.up * 2f;
        Vector3 closeOffset = targetPos - playerObj.forward * 2f + Vector3.up * 1f;

        // Elegir offset según distancia y suavizar transición
        Vector3 desiredOffset = distance < minDistanceToSwitch ? closeOffset : playerOffset;
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, desiredOffset, Time.deltaTime * rotationSpeed);

        // Configurar cámara
        cam.Follow = cameraTarget;
        cam.LookAt = target;

        // Rotar el jugador hacia el objetivo
        Vector3 viewDir = targetPos - playerObj.position;
        viewDir.y = 0;
        if (viewDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(viewDir);
            if (rotationOffset != Vector3.zero)
                targetRotation *= Quaternion.Euler(rotationOffset);

            playerObj.rotation = Quaternion.Slerp(playerObj.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }



}