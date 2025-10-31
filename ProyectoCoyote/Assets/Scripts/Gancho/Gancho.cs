using NUnit.Framework;
using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Gancho : MonoBehaviour
{
    private GameInput gameInput;
    [SerializeField] LayerMask targetLayers;
    Transform HookableObjectLocator;
    Transform cam;
    public Transform currentTarget;
    [Header("Settings")]
    [SerializeField] bool zeroVert_Look;
    [SerializeField] float maxNoticeZone= 20;
    [SerializeField] float minNoticeZone = 5;
    [SerializeField] float lookAtSmoothing;
    [Tooltip("Angle_Degree")][SerializeField] float maxNoticeAngle = 120;

    [Header("When selected")]
    [SerializeField]  float MovingTargetFinalDistanceInFront = 4;
    [SerializeField]  Vector3 offsetDistanceWhenSelected = new Vector3(0, 0, 0);

    HookableObject hookableObject;
    EnemyLockOn lockOn;
    PlayerMovement movement;
    GameObject player;
    CameraController CamControl;
    Transform HookCanvas;
    
    public bool selectingHook;
    public bool isHooked;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameInput = FindAnyObjectByType<GameInput>();
        CamControl = FindAnyObjectByType<CameraController>();
        HookableObjectLocator = GameObject.Find("HookableObjectLocator").transform;
        movement = FindAnyObjectByType<PlayerMovement>();
        HookCanvas = GameObject.Find("HookCanvas").transform;
        lockOn = FindAnyObjectByType<EnemyLockOn>();
        player = GameObject.Find("Player");
        HookCanvas.gameObject.SetActive(false);
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("Camera.main is null at Start. Delaying cam assignment.");
            StartCoroutine(AssignCameraLater());
        }
        currentTarget = null;
        selectingHook = false;
        isHooked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameInput.HookPressed) 
        {
            Debug.Log("Activando el gancho...");
            ActivateTargetHook(); 
        }

            HookLogic();


        if (currentTarget) 
        { 
            LookAtTarget(); 
        }
        
    }
    public void HookLogic()
    {
        
        // Navegación por los objetos enganchables
        if (selectingHook) { 
        
            if (Input.GetKeyDown(KeyCode.W))
                currentTarget = FindDirectionalTarget(false, true);
            else if (Input.GetKeyDown(KeyCode.S))
                currentTarget = FindDirectionalTarget(false, false);
            else if (Input.GetKeyDown(KeyCode.D))
                currentTarget = FindDirectionalTarget(true, false);
            else if (Input.GetKeyDown(KeyCode.A))
                currentTarget = FindDirectionalTarget(false, false);
        }
        // Selecionar objeto
        if (Input.GetMouseButtonDown(0)) SelectTarget();
        if (isHooked) 
        {
            if (Input.GetKeyDown(KeyCode.S))
                AtractTarget();
            else if (Input.GetKeyDown(KeyCode.W))
                GoToTarget();
        }
    }

    

    public void ActivateTargetHook()
    {
        if (lockOn.enemyLocked) return;
        if (currentTarget != null) // Si ya hay un objeto enganchable, resetear
        {
            ResetTarget();
            CamControl.ActiveFollowCamera();

            return;
        }

        currentTarget = ScanNearBy();
        if (currentTarget != null) 
        {
            movement.startHookMode();
            HookCanvas.gameObject.SetActive(true);
            CamControl.ActiveHookCamera();
            selectingHook = true;
            lockOn.enemyLocked = false;
            Debug.Log("----------Cámara gancho Activada");
        } 
           
    }
    void ResetTarget()
    {
        movement.stopHookMode();
        HookCanvas.gameObject.SetActive(false);
        currentTarget = null;
        selectingHook = false;
        isHooked = false;
        Image img = HookCanvas.GetComponentInChildren<Image>();
        img.color = Color.white;
        if(!lockOn.enemyLocked) CamControl.ActiveFollowCamera();
        Debug.Log("Se ha desactivado el gancho. Volviendo a modo libre");
    }

    #region Calcular Objetos Enganchables
    /*
     * Calcular el objetivo más cercano al objeto fijado en función de si está a la derecha(toRight = true) o a la izquierda(toRight = false)
     */

    private Transform FindDirectionalTarget(bool toRight, bool toUp)
    {
        if (currentTarget == null) return null;

        Collider[] candidates = Physics.OverlapSphere(currentTarget.position, maxNoticeZone, targetLayers);
        Transform bestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (var c in candidates)
        {
            Transform candidate = c.transform;
            if (candidate == currentTarget) continue;

            Vector3 offset = candidate.position - currentTarget.position;
            float distance = offset.magnitude;

            // Dirección horizontal relativa a la cámara
            Vector3 rightDir = cam.right;
            float dotRight = Vector3.Dot(offset.normalized, rightDir);

            // Dirección vertical global
            float verticalOffset = offset.y;

            bool isValid = false;

            if (toRight || !toRight) // se ha pulsado A o D
            {
                if (toRight && dotRight > 0.5f) isValid = true;
                if (!toRight && dotRight < -0.5f) isValid = true;
            }

            if (toUp || !toUp) // se ha pulsado W o S
            {
                if (toUp && verticalOffset > 0.5f) isValid = true;
                if (!toUp && verticalOffset < -0.5f) isValid = true;
            }

            if (!isValid) continue;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = candidate;
            }
        }

        return bestTarget != null ? bestTarget : currentTarget;
    }

    private Transform ScanNearBy()
    {
        // Crea una esfera al rededor del personaje con radio en noticeZone.
        // Guarda en un array todos los objetos que coincidan con la target
        // definida en targetLayers.
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, maxNoticeZone, targetLayers);

        // Inicializa las variables para encontrar el objetivo m�s cercano.
        float closestAngle = maxNoticeAngle;
        Transform closestTarget = null;

        // Si no hay objetivos cerca, se sale.
        if (nearbyTargets.Length <= 0)
        {
            Debug.Log("No se han encontrado enemigos cerca!");
            return null;
        }


        // Recorre todos los objetivos detectados y calcula su direcci�n y 
        // �ngulo desde la c�mara, detecta al m�s cercano.
        for (int i = 0; i < nearbyTargets.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, nearbyTargets[i].transform.position);

            // Ignora si está demasiado cerca
            if (distance < minNoticeZone)
                continue;

            Vector3 dir = nearbyTargets[i].transform.position - cam.position;
            dir.y = 0;
            float _angle = Vector3.Angle(cam.forward, dir);

            if (_angle < closestAngle)
            {
                closestTarget = nearbyTargets[i].transform;
                closestAngle = _angle;
            }
        }

        // Si no hay objetivos cerca, se sale.
        if (!closestTarget)
        {
            Debug.Log("No se han encontrado enemigos cerca!");
            return null;
        }

        
        // Devuelve el enemigo v�lido
        return closestTarget;
    }


    // Mirar al objeto
    private void LookAtTarget()
    {
        // Si desaparece el enemigo al que estamos mirando, reasignar enemigo
        if (currentTarget == null)
        {
            return;
        }

        // Actaliza la posici�n del localizador del enemigo
        
        HookableObjectLocator.position = currentTarget.position;

       
    }
    IEnumerator AssignCameraLater()
    {
        yield return new WaitForSeconds(0.1f); // espera breve
        cam = Camera.main?.transform;
        if (cam == null)
        {
            Debug.LogError("Camera.main still null after delay.");
        }
    }
    #endregion

    #region Seleccionar objeto
    public HookableObject GetHookableObject()
    {
        HookableObject targetObject = null;
        if (currentTarget != null)
        {
            targetObject = currentTarget.GetComponent<HookableObject>();
            if (targetObject != null)
            {
                // Ya tienes acceso al objeto HookableObject
                Debug.Log("HookableObject encontrado: " + targetObject.name);
            }
            else
            {
                Debug.LogWarning("El objeto actual no tiene componente HookableObject.");
            }
        }
        return targetObject;
        
    }
    private void SelectTarget()
    {
        hookableObject = GetHookableObject();
        if (hookableObject) 
        { 
            isHooked = true;
            selectingHook = false;
            Image img = HookCanvas.GetComponentInChildren<Image>();
            img.color = Color.red;
        }
    }
    private void GoToTarget()
    {
        if (currentTarget == null) return;
        // Dirección desde el objeto hacia la cámara
        Vector3 directionToCamera = (cam.transform.position - currentTarget.position).normalized;
        // POSICIÓN FINAL
        Vector3 targetPosition = currentTarget.position + directionToCamera * MovingTargetFinalDistanceInFront; 

       

        // Mover el Rigidbody del jugador
        var rb = player.GetComponent<Rigidbody>();
        rb.MovePosition(targetPosition);

        // Activar modo enemigo si aplica
        if (currentTarget.gameObject.GetComponent<Enemy>())
        {
            Debug.Log("Es enemigo");
            lockOn.ActivateLockMode();
        }

        ResetTarget();

    }

    private void AtractTarget()
    {
        
        if (hookableObject.canBeHooked)
        {
            Vector3 directionToCamera = (cam.transform.position - currentTarget.position).normalized;
            Vector3 targetPosition = cam.transform.position + directionToCamera * -MovingTargetFinalDistanceInFront;
            currentTarget.position = targetPosition;

            if (currentTarget.gameObject.GetComponent<Enemy>())
            {
                Debug.Log("Es enemigo");
                lockOn.ActivateLockMode();
            }
        }
        else 
        {
            CamControl.StartShake();
        }

       
        
        ResetTarget();
    }

    #endregion
}
