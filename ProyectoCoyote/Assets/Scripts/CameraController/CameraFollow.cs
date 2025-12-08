using Services;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public CinemachineCamera freeCamera;
    public CinemachineCamera lockOnCamera;
    public CinemachineCamera hookCamera;
    public Transform cameraTarget;
    public Transform HookableObjectLocator;

    Transform player;
    Transform playerObj;
    EnemyLockOn enemyLockOn;
    Gancho hook;
    HandleOcclusions handleOcclusions;

    [Header("Initial Settings")]
    public Vector3 initialFacingDirection = Vector3.forward;
    [Header("Settings")]
    public float rotationSpeed = 5f;
    public float minDistanceToSwitch = 3f;
    private bool lockedCamera;
    private bool hookedCamera;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        playerObj = GameObject.Find("Player/Player_02").transform;
        HookableObjectLocator = GameObject.Find("HookableObjectLocator").transform;
        enemyLockOn = GameObject.FindAnyObjectByType<EnemyLockOn>();
        hook = GameObject.FindAnyObjectByType<Gancho>();
        handleOcclusions = GameObject.FindAnyObjectByType<HandleOcclusions>();
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeToRestart(InitialCameraDirection);
        ServiceLocator.Instance.Get<IGameStateManager>().subscribeCombatAreaChange(AreaChange);

        InitialCameraDirection();


    }

    private void AreaChange(combatAreaManager manager, WaveData data)
    {
        setInitialCameraDirection(data.initCameraDirection);

    }

    private void LateUpdate()
    {
        lockedCamera = enemyLockOn != null && enemyLockOn.enemyLocked;
        hookedCamera = hook != null && hook.selectingHook;

        if (lockedCamera) HandleLockCamera(enemyLockOn.currentTarget, lockOnCamera);
        else if (hookedCamera) HandleTarget(hook.currentTarget, hookCamera);
        else RotateFreePlayer();

        // handleOcclusions.HandleTransparency();

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

    private void HandleLockCamera(Transform enemy, CinemachineCamera cam, Vector3 rotationOffset = default)
    {
        if (enemy == null || cam == null) return;
        
        // Rotar el jugador hacia el enemigo
        Vector3 lookToEnemy = enemy.position - playerObj.position;
        lookToEnemy.y = 0f;
        if (lookToEnemy.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookToEnemy);
            if (rotationOffset != Vector3.zero)
                targetRotation *= Quaternion.Euler(rotationOffset);

            playerObj.rotation = Quaternion.Slerp(playerObj.rotation, targetRotation, Time.deltaTime * rotationSpeed);
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

    public void InitialCameraDirection() 
    {
        if (initialFacingDirection != Vector3.zero)
        {
            Quaternion startRotation = Quaternion.LookRotation(initialFacingDirection.normalized, Vector3.up);
            playerObj.rotation = startRotation;

            cameraTarget.position = playerObj.position + Vector3.up * 2f;

            var orbitalFollow = freeCamera.GetComponent<CinemachineOrbitalFollow>();
            if (orbitalFollow != null)
            {
                float targetYaw = startRotation.eulerAngles.y;

                orbitalFollow.HorizontalAxis.Value = targetYaw;

                freeCamera.ForceCameraPosition(
                    cameraTarget.position + startRotation * Vector3.back * 5f, // Ajusta la distancia según necesites
                    startRotation
                );
            }
        }

    }
    public void AlignFreeCameraBehindPlayer()
    {
        var orbitalFollow = freeCamera.GetComponent<CinemachineOrbitalFollow>();
        if (orbitalFollow != null)
        {
            // Yaw del jugador
            float targetYaw = playerObj.eulerAngles.y;

            // En vez de forzar, asigna el valor del eje
            orbitalFollow.HorizontalAxis.Value = targetYaw;
        }

        // Posición del target de cámara
        cameraTarget.position = playerObj.position + Vector3.up * 2f;
    }

    public void setInitialCameraDirection(Vector3 initialCameraDirection)
    {
        this.initialFacingDirection = initialCameraDirection;
    }

}