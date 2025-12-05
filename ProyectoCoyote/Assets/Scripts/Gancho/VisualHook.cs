using NUnit.Framework;
using System;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR;

public class VisualHook : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] public float cableSpeed = 50f;
    [SerializeField] private float handOffset = 0.5f;
    [SerializeField] private Transform leftHand;
    [SerializeField] private float retractOffset = 0.5f;

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
            ResetVisualHook();
        }
    }

    public void RetractHookAtractTarget()
    {
        Debug.Log("target = " + target);
        if (target != null)
        {

            currentState = HookState.RetractingWithTarget;
        }
        else 
        {
            ResetVisualHook();
        }
        
    }

    public void RetractHookGoToTarget()
    {
        if (target != null)
        {
            currentState = HookState.GoingToTarget;
        }else
        {
            ResetVisualHook();
        }

    }

    // ----------------- L�GICA DE ESTADOS -----------------
    private void UpdateIdle()
    {
        if (target)
        {
            lineRenderer.SetPosition(1, target.position);
        }
        else
        { 
            lineRenderer.enabled = false; 
            ResetVisualHook();
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
            
            var hookableObject = hook.GetHookableObject();
            if (hookableObject.Dodge)
            {
                currentState = HookState.Retracting;
                hookableObject.dodgeHook();

            }
            else
            {
                hookableObject.getHook();
                currentState = HookState.Idle;
            }
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
            if(hook.GetHookableObject() != null && hook.GetHookableObject().Dodge)
            {
                hook.WaitForHookFinish();
            }
            ResetVisualHook();
            currentState = HookState.Idle;
        }
    }

    private void UpdateRetractCableWithTarget()
    {
        Debug.Log("Atraer objetivo");

        float enemyRadius = GetTargetRadius(target);
        float playerRadius = GetTargetRadius(player.transform);

        float optimalDistance = enemyRadius + playerRadius + retractOffset;

        Vector3 cameraForward = cam.transform.forward;
        cameraForward.Normalize();

        Vector3 frontOfPlayer = player.transform.position + (cameraForward * optimalDistance);

        // Mueve el target hacia esa posici�n
        target.position = Vector3.MoveTowards(
            target.position, 
            frontOfPlayer, 
            cableSpeed * Time.deltaTime
            );

        lineRenderer.SetPosition(0, GetHookOrigin());
        lineRenderer.SetPosition(1, target.position);

        if (Vector3.Distance(target.position, frontOfPlayer) <= optimalDistance)
        {
            lineRenderer.enabled = false;
            hook.WaitForHookFinish();
            ResetVisualHook();
            currentState = HookState.Idle;
            hook.GetHookableObject().endHook();
        }
    }

    private void UpdateGoToTarget()
    {
        Debug.Log("Ir a objetivo");

        float enemyRadius = GetTargetRadius(target);
        float playerRadius = GetTargetRadius(player.transform);

        float optimalDistance = enemyRadius + playerRadius + retractOffset;

        Vector3 cameraForward = cam.transform.forward;
        cameraForward.Normalize();

        Vector3 frontOfTarget = target.transform.position - (cameraForward * optimalDistance);

        // Mueve al jugador hacia esa posici�n
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
            ResetVisualHook();
            currentState = HookState.Idle;
        }
    }


    private void ResetVisualHook()
    {
        currentCableLength = 0f;
        target = null;
        lineRenderer.enabled = false;
        
    }


    public float GetRetractTime()
    {
        return currentCableLength / cableSpeed;
    }

    private float GetTargetRadius(Transform targetTransform)
    {
        if (targetTransform == null) return 0.5f;

        // Intentar obtener diferentes tipos de colliders
        CapsuleCollider capsule = targetTransform.GetComponent<CapsuleCollider>();
        if (capsule != null)
        {
            return capsule.radius;
        }

        //// Fallback: usar bounds del renderer
        //Renderer renderer = targetTransform.GetComponent<Renderer>();
        //if (renderer != null)
        //{
        //    return renderer.bounds.extents.magnitude * 0.5f;
        //}
        else
        return 0.5f;
    }
    // var rb = player.GetComponent<Rigidbody>();
    // rb.MovePosition(currentEnd);

}
