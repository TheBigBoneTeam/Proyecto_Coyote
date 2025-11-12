using NUnit.Framework;
using System;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;
using UnityEngine.XR;

public class VisualHook : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private float cableSpeed = 50f;
    [SerializeField] private float handOffset = 0.5f;
    [SerializeField] private Transform leftHand;

    private Transform target;
    private GameObject player;
    private float currentCableLength;
    private CameraController CamControl;
    private Transform cam;
    private EnemyLockOn lockOn;
    private Gancho hook;

    // Estados del gancho
    private enum HookState { Idle, Extending, Retracting, RetractingWithTarget, GoingToTarget }
    private HookState currentState = HookState.Idle;

    private float retractOffset;
    public bool visualHookFinished { get; private set; } = false;

    void Start()
    {
        cam = Camera.main.transform;
        CamControl = FindAnyObjectByType<CameraController>();
        lockOn = FindAnyObjectByType<EnemyLockOn>();
        player = GameObject.Find("Player");

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
        lineRenderer.widthMultiplier = 0.05f;
    }

    void Update()
    {
        Debug.Log("Estado actual: " + currentState);
        switch (currentState)
        {
            case HookState.Extending:
                UpdateExtendCable();
                break;
            case HookState.Retracting:
                UpdateRetractCable();
                break;
            case HookState.RetractingWithTarget:
                Debug.Log("Estado: Atraer objetivo");
                UpdateRetractCableWithTarget();
                break;
            case HookState.GoingToTarget:
                UpdateGoToTarget();
                break;
        }
    }

    private Vector3 GetHookOrigin()
    {
        return leftHand != null ? leftHand.position : player.transform.position;
    }

    public void ThrowHook(Transform targetTransform)
    {
        Debug.Log("Se ha lanzado el gancho....");
        target = targetTransform;
        currentCableLength = 0f;

        lineRenderer.enabled = true;
        currentState = HookState.Extending;
    }

    public void RetractHook()
    {
        if (target != null)
        {
            currentState = HookState.Retracting;
        }
        else
        ResetCamera();
    }

    public void RetractHookAtractTarget(float offset)
    {
        Debug.Log("target = " + target);
        if (target != null)
        {
            retractOffset = offset;
            currentCableLength = Vector3.Distance(GetHookOrigin(), target.position);
            lineRenderer.enabled = true;
            visualHookFinished = false;
            currentState = HookState.RetractingWithTarget;
            Debug.Log("Estado cambiado a RetractingWithTarget");
        } else
        ResetCamera();
    }

    public void RetractHookGoToTarget(float offset)
    {
        if (target != null)
        {
            retractOffset = offset;
            currentState = HookState.GoingToTarget;
        }else
            ResetCamera();
        
    }

    // ----------------- LÓGICA DE ESTADOS -----------------

    private void UpdateExtendCable()
    {
        Vector3 start = GetHookOrigin();
        Vector3 end = target.position;
        float totalDistance = Vector3.Distance(start, end);

        currentCableLength = Mathf.MoveTowards(currentCableLength, totalDistance, cableSpeed * Time.deltaTime);

        Vector3 direction = (end - start).normalized;
        Vector3 currentEnd = start + direction * currentCableLength;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, currentEnd);

        if (currentCableLength >= totalDistance)
        {
            lineRenderer.SetPosition(1, end);
            currentState = HookState.Idle;
        }
    }

    private void UpdateRetractCable()
    {
        Vector3 start = GetHookOrigin();
        Vector3 end = target.position;

        currentCableLength = Mathf.MoveTowards(currentCableLength, 0f, cableSpeed * Time.deltaTime);

        Vector3 direction = (end - start).normalized;
        Vector3 currentEnd = start + direction * currentCableLength;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, currentEnd);

        if (currentCableLength <= 0f)
        {
            lineRenderer.enabled = false;
            currentState = HookState.Idle;
        }
    }

    private void UpdateRetractCableWithTarget()
    {
        Debug.Log("Atraer objetivo");
        Vector3 frontOfPlayer = player.transform.position + player.transform.forward * retractOffset;

        // Mueve el target hacia esa posición
        target.position = Vector3.MoveTowards(
            target.position, 
            frontOfPlayer, 
            cableSpeed * Time.deltaTime
            );

        lineRenderer.SetPosition(0, GetHookOrigin());
        lineRenderer.SetPosition(1, target.position);

        if (Vector3.Distance(target.position, frontOfPlayer) <= 0.05f)
        {
            lineRenderer.enabled = false;
            visualHookFinished = true;
            hook.WaitForHookFinish();
            currentState = HookState.Idle;
        }
    }

    private void UpdateGoToTarget()
    {
        Debug.Log("Ir a objetivo");
        Vector3 frontOfTarget = target.position + target.forward * retractOffset;

        // Mueve al jugador hacia esa posición
        player.transform.position = Vector3.MoveTowards(
            player.transform.position, 
            frontOfTarget, 
            cableSpeed * Time.deltaTime
            );

        lineRenderer.SetPosition(0, target.position);
        lineRenderer.SetPosition(1, player.transform.position);

        if (Vector3.Distance(player.transform.position, frontOfTarget) <= 0.05f)
        {
            lineRenderer.enabled = false;
            currentState = HookState.Idle;
        }
    }


    private void ResetCamera()
    {
        if (!lockOn.enemyLocked) CamControl.ActiveFollowCamera();
    }





    // var rb = player.GetComponent<Rigidbody>();
    // rb.MovePosition(currentEnd);

}
