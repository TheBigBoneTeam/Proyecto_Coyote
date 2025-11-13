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

    private Transform target = null;
    private GameObject player;
    private float currentCableLength;
    private CameraController CamControl;
    private Transform cam;
    private EnemyLockOn lockOn;
    private Gancho hook;
    private Rigidbody rb;
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
        rb = player.GetComponent<Rigidbody>();
        hook = FindAnyObjectByType<Gancho>();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
        lineRenderer.widthMultiplier = 0.05f;
    }

    void Update()
    {
        // Debug.Log("Estado actual: " + currentState);
        switch (currentState)
        {
            case HookState.Extending:
                UpdateExtendCable();
                break;
            case HookState.Retracting:
                UpdateRetractCable();
                break;
            case HookState.RetractingWithTarget:
                UpdateRetractCableWithTarget();
                break;
            case HookState.GoingToTarget:
                UpdateGoToTarget();
                break;
            case HookState.Idle:
                UpdateIdle();
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
        {
            ResetCamera();
        }
    }

    public void RetractHookAtractTarget(float offset)
    {
        Debug.Log("target = " + target);
        if (target != null)
        {
            retractOffset = offset;
            currentState = HookState.RetractingWithTarget;
        }
        else 
        {
            ResetCamera();
        }
        
    }

    public void RetractHookGoToTarget(float offset)
    {
        if (target != null)
        {
            retractOffset = offset;
            currentState = HookState.GoingToTarget;
        }else
        {
            ResetCamera();
        }

    }

    // ----------------- LÓGICA DE ESTADOS -----------------
    private void UpdateIdle()
    {
        if (target.position != null)
        {
            lineRenderer.SetPosition(1, target.position);
        }
        else
        { 
            lineRenderer.enabled = false; 
            ResetCamera();
        }
    }
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
            ResetCamera();
            currentState = HookState.Idle;
        }
    }

    private void UpdateRetractCableWithTarget()
    {
        Debug.Log("Atraer objetivo");
        Vector3 directionToCamera = (cam.transform.position - target.position).normalized;
        Vector3 frontOfPlayer = cam.transform.position - (directionToCamera * retractOffset);

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
            hook.WaitForHookFinish();
            currentState = HookState.Idle;
        }
    }

    private void UpdateGoToTarget()
    {
        Debug.Log("Ir a objetivo");
        Vector3 directionToCamera = (cam.transform.position - target.position).normalized;
        Vector3 frontOfTarget = target.transform.position + (directionToCamera * retractOffset);

        // Mueve al jugador hacia esa posición
        rb.MovePosition(Vector3.MoveTowards(
            rb.position, 
            frontOfTarget, 
            cableSpeed * Time.deltaTime
            ));

        lineRenderer.SetPosition(0, target.position);
        lineRenderer.SetPosition(1, rb.position);

        if (Vector3.Distance(rb.position, frontOfTarget) <= 0.05f)
        {
            lineRenderer.enabled = false;
            hook.WaitForHookFinish();
            currentState = HookState.Idle;
        }
    }


    private void ResetCamera()
    {
        currentCableLength = 0f;
        target = null;
        lineRenderer.enabled = false;
        if (!lockOn.enemyLocked) CamControl.ActiveFollowCamera();
    }





    // var rb = player.GetComponent<Rigidbody>();
    // rb.MovePosition(currentEnd);

}
