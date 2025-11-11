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
    CameraController CamControl;
    Transform cam;
    EnemyLockOn lockOn;


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
    }
    private Vector3 GetHookOrigin()
    {
        if (leftHand != null)
        {
            
            return leftHand.position;
        }
        else 
        {
            Debug.Log("Mano no encontrada");
            return Vector3.zero;
            
        } 
            
        
    }

    public void ThrowHook(Transform targetTransform)
    {
        Debug.Log("Se ha lanzado el gancho....");
        target = targetTransform;
        currentCableLength = 0f;

        lineRenderer.enabled = true;
        StartCoroutine(ExtendCable());
    }
    
    public void RetractHook()
    {
        if (target != null) 
        { 
            StartCoroutine(RetractCable());
        }
        else
        {
            if (!lockOn.enemyLocked) CamControl.ActiveFollowCamera();

        }
    }

    public void RetractHookAtractTarget(float offset)
    {
        if (target != null)
        {
            StartCoroutine(RetractCableWithTarget(offset));

        }
        else
        {
            if (!lockOn.enemyLocked) CamControl.ActiveFollowCamera();

        }
    }
    public void RetractHookGoToTarget(float offset)
    {
        if (target != null)
        {
            StartCoroutine(GoToTarget(offset));

        }
        else
        {
            if (!lockOn.enemyLocked) CamControl.ActiveFollowCamera();

        }
    }


    private IEnumerator ExtendCable()
    {
        Vector3 Origin = GetHookOrigin();
        Vector3 start = Origin!=Vector3.zero ? Origin : player.transform.position;
        Vector3 end = target.position;
        float totalDistance = Vector3.Distance(start, end);

        while (currentCableLength < totalDistance)
        {
            currentCableLength = Mathf.MoveTowards(
                currentCableLength,
                totalDistance,
                cableSpeed * Time.deltaTime
            );

            Vector3 direction = (end - start).normalized;
            Vector3 currentEnd = start + direction * currentCableLength;

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, currentEnd);

            yield return null;
        }

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
    private IEnumerator RetractCable()
    {
        Vector3 Origin = GetHookOrigin();
        Vector3 start = Origin != Vector3.zero ? Origin : player.transform.position;
        Vector3 end = target.position;
        float totalDistance = Vector3.Distance(start, end);

        currentCableLength = totalDistance;

        while (currentCableLength > 0f)
        {
            // Reducir la longitud del cable
            currentCableLength = Mathf.MoveTowards(
                currentCableLength,
                0f,
                cableSpeed * Time.deltaTime
            );

            // Dirección del cable
            Vector3 direction = (end - start).normalized;
            Vector3 currentEnd = start + direction * currentCableLength;

            // Actualizar cable
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, currentEnd);

            yield return null;
        }
        
        lineRenderer.enabled = false;
        if (!lockOn.enemyLocked) CamControl.ActiveFollowCamera();
    }
    private IEnumerator RetractCableWithTarget(float offset)
    {
        Vector3 Origin = GetHookOrigin();
        Vector3 start = Origin != Vector3.zero ? Origin : player.transform.position;
        Vector3 end = target.position;
        float totalDistance = Vector3.Distance(start, end);

        currentCableLength = totalDistance;

        while (currentCableLength > offset) 
        {
            currentCableLength = Mathf.MoveTowards(
                currentCableLength,
                offset, 
                cableSpeed * Time.deltaTime
            );

            Vector3 direction = (end - start).normalized;
            Vector3 currentEnd = start + direction * currentCableLength;

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, currentEnd);

            target.position = currentEnd;

            yield return null;
        }

        lineRenderer.enabled = false;
        if (!lockOn.enemyLocked) CamControl.ActiveFollowCamera();
    }

    private IEnumerator GoToTarget(float offset)
    {
        Vector3 Origin = GetHookOrigin();
        Vector3 end = Origin != Vector3.zero ? Origin : player.transform.position;
        Vector3 start = target.position;
        float totalDistance = Vector3.Distance(start, end);

        currentCableLength = totalDistance;

        lineRenderer.enabled = false;

        while (currentCableLength > offset)
        {
            currentCableLength = Mathf.MoveTowards(
                offset, currentCableLength,
                cableSpeed * Time.deltaTime
            );

            Vector3 direction = (end - start).normalized;
            Vector3 currentEnd = start + direction * currentCableLength;

            //lineRenderer.SetPosition(0, currentEnd);
            //lineRenderer.SetPosition(1, start);

            var rb = player.GetComponent<Rigidbody>();
            rb.MovePosition(currentEnd);

            yield return null;
        }

        lineRenderer.enabled = false;
        if (!lockOn.enemyLocked) CamControl.ActiveFollowCamera();
    }




    // var rb = player.GetComponent<Rigidbody>();
    // rb.MovePosition(currentEnd);

}
