using NUnit.Framework;
using System;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Gancho : MonoBehaviour
{
    private GameInput gameInput;
    [SerializeField] LayerMask targetLayers;
    Transform HookableObjectLocator;
    Transform cam;
    public Transform currentTarget;
    [Header("Settings")]
    [SerializeField] bool zeroVert_Look;
    [SerializeField] float maxNoticeZone= 100;
    [SerializeField] float minNoticeZone = 10;
    [SerializeField] float lookAtSmoothing;
    [SerializeField] public Vector3 lookAtRotationOffset = new Vector3(0,0,0);
    [Tooltip("Angle_Degree")][SerializeField] float maxNoticeAngle = 120;
    [SerializeField] int cooldown = 5;
    private TextMeshProUGUI _cooldownUIText;
    private bool _canUseHook = false;

    [Header("When selected")]
    [SerializeField]  float OffsetFinalPos = 0;

    VisualHook visualHook;
    HookableObject hookableObject;
    EnemyLockOn lockOn;
    PlayerMovement movement;
    GameObject player;
    CameraController CamControl;
    Transform HookCanvas;
    HookController hookController;
    private Image _hookImageUI;
    HookObserver hookObserver;
    
    public bool selectingHook;
    public bool isHooked;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        gameInput = FindAnyObjectByType<GameInput>();
        CamControl = FindAnyObjectByType<CameraController>();
        HookableObjectLocator = GameObject.Find("HookableObjectLocator").transform;
        movement = FindAnyObjectByType<PlayerMovement>();
        HookCanvas = GameObject.Find("HookUI").transform;
        _hookImageUI = HookCanvas.Find("HookImage").
            GetComponent<Image>();
        _cooldownUIText = HookCanvas.Find("CooldownText").
            GetComponent<TextMeshProUGUI>(); 
        visualHook = FindAnyObjectByType<VisualHook>();

        lockOn = FindAnyObjectByType<EnemyLockOn>();
        player = GameObject.Find("Player");
        hookController = FindAnyObjectByType<HookController>();
        // Obsever prueba
        // hookObserver = FindAnyObjectByType<HookObserver>();
        // hookObserver.Configure(hookController);
        //

        _hookImageUI.gameObject.SetActive(false);
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("Camera.main is null at Start. Delaying cam assignment.");
            StartCoroutine(AssignCameraLater());
        }
        
        _canUseHook = true;
        currentTarget = null;
        selectingHook = false;
        isHooked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameInput.HookAimPressed && _canUseHook)
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
        
            if (gameInput.Hook_TPPressed)
            {
                currentTarget = FindDirectionalTarget(false, true);
            }
            else if (gameInput.HookDisconfirmPressed)
            {
                currentTarget = FindDirectionalTarget(false, false);
            }
            else if (gameInput.HookSelectRightPressed)
            {
                currentTarget = FindDirectionalTarget(true, false);
            }
            else if (gameInput.HookSelectLeftPressed)
            {
                currentTarget = FindDirectionalTarget(false, false);
            }
        }
        // Selecionar objeto 
        if (gameInput.HookConfirmPressed && selectingHook)
        {
            SelectTarget();
            movement.animator.CrossFade("Grapple_03", 0.2f);
        }

        if (isHooked) 
        {
            if (gameInput.HookAttractPressed) 
            { 
                AtractTarget();
                StartCoroutine(Cooldown());
                hookController.HookUsed();
            }
            else if (gameInput.Hook_TPPressed)
            {
                GoToTarget();
                StartCoroutine(Cooldown());
                hookController.HookUsed();
            }
        }
    }

    

    public void ActivateTargetHook()
    {
        if (lockOn.enemyLocked) return;
        if (currentTarget != null) // Si ya hay un objeto enganchable, resetear
        {
            ResetTarget();
            CamControl.ActiveFollowCamera();
            movement.animator.CrossFade("Idle_01", 0.2f);
            return;
        }

        currentTarget = ScanNearBy();
        if (currentTarget != null) 
        {
            movement.startHookMode();
            _hookImageUI.gameObject.SetActive(true);
            CamControl.ActiveHookCamera();

            selectingHook = true;
            lockOn.enemyLocked = false;
            Debug.Log("----------Cámara gancho Activada");
            movement.animator.CrossFade("Grapple_01", 0.2f);
            LookAtTarget();
        } 
           
    }
    void ResetTarget()
    {
        movement.stopHookMode();
        _hookImageUI.gameObject.SetActive(false);
        visualHook.RetractHook();
        currentTarget = null;
        selectingHook = false;
        isHooked = false;
        _hookImageUI.color = Color.white;
        
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

            //if (Blocked(candidate.position))
            //{
            //    Debug.Log("Hay algo bloqueando el objeto");
            //    isValid = false;
            //}

            if (!isValid) continue;

            if (distance < closestDistance && !Blocked(candidate.position))
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
            Debug.Log("No se han encontrado objetos válidos!");
            return null;
        }

        // Si hay algun elemento de la escena bloqueando la visi�n del jugador, se sale.
        if (Blocked(closestTarget.position))
        {
            Debug.Log("Hay algo bloqueando el objeto");
            return null;
        }

        // Devuelve el enemigo v�lido
        return closestTarget;
    }

    // Detectar si hay un objeto bloqueando las escena
    bool Blocked(Vector3 targetPosition)
    {
        Vector3 origin = cam.transform.position;//  + Vector3.up * 1.5f; // desde el pecho del jugador
        Vector3 direction = targetPosition - origin;
        float distance = direction.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(origin, direction.normalized, distance);

        foreach (RaycastHit hit in hits)
        {
            // Ignora el jugador y el objetivo
            if (hit.transform == currentTarget || hit.transform == transform)
                continue;

            // Ignora objetos sin collider físico o con capas ignoradas
            if (((1 << hit.transform.gameObject.layer) & targetLayers) == 0)
            {
                Debug.Log($"Bloqueado por: {hit.transform.name}");
                return true;
            }
        }

        return false;
    }

    // Mirar al objeto
    private void LookAtTarget()
    {
        if (currentTarget == null)
        {
            return;
        }

        // Actaliza la posici�n del localizador del target
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

            DisableCollisions();


            selectingHook = false;
            _hookImageUI.color = Color.red;
            visualHook.ThrowHook(currentTarget);
        }
    }
    private void GoToTarget()
    {
        if (currentTarget == null) return;

        //// Dirección desde el objeto hacia la cámara
        //Vector3 directionToCamera = (cam.transform.position - currentTarget.position).normalized;
        //// POSICIÓN FINAL
        //Vector3 targetPosition = currentTarget.position + directionToCamera * OffsetFinalPos;

        //// Mover el Rigidbody del jugador
        //var rb = player.GetComponent<Rigidbody>();
        //rb.MovePosition(targetPosition);


        visualHook.RetractHookGoToTarget(OffsetFinalPos);
        movement.animator.CrossFade("Grapple_04", 0.2f);
        

    }

    private void AtractTarget()
    {

        if (hookableObject.canBeHooked)
        {
            
            visualHook.RetractHookAtractTarget(OffsetFinalPos);
            //Vector3 directionToCamera = (cam.transform.position - currentTarget.position).normalized;
            //Vector3 targetPosition = cam.transform.position + directionToCamera * -OffsetFinalPos;
            //currentTarget.position = targetPosition;
            movement.animator.CrossFade("Grapple_04", 0.2f);


        }
        else 
        {
            CamControl.StartShake();
            ResetTarget();
        }

       
        
        
    }

    #endregion

    private IEnumerator Cooldown()
    {
        _canUseHook = false;
        _cooldownUIText.SetText($"Cooldown Hook: active");

        // yield return new WaitForSeconds(cooldown);
        int i = cooldown;

        while (i > 0)
        {
            _cooldownUIText.SetText($"Cooldown Hook: {i}");
            yield return new WaitForSeconds(1);
            i--;
        }
        _canUseHook = true;
        _cooldownUIText.SetText("");

    }

    public void DisableCollisions()
    {
        int targetLayer = currentTarget.gameObject.layer;

        // Max Layer = 32
        for (int i = 0; i < 32; i++)
        {
            if (i != LayerMask.NameToLayer("ahatIsGround")) 
            {
                Physics.IgnoreLayerCollision(targetLayer, i, true);
            }
        }
    }

    
    public void EnableAllCollisions()
    {
        int targetLayer = currentTarget.gameObject.layer;

        for (int i = 0; i < 32; i++)
        {
            Physics.IgnoreLayerCollision(targetLayer, i, false);
        }
    }

    public void WaitForHookFinish()
    {
        currentTarget.gameObject.GetComponent<Collider>().enabled = true;
        EnableAllCollisions();

        Debug.Log("Ha llegado a su destino");
        if (currentTarget.gameObject.GetComponent<Enemy>())
        {
            Debug.Log("Es enemigo");
            lockOn.currentTarget = currentTarget;
            lockOn.FoundTarget();
        }
        if (!lockOn.enemyLocked) CamControl.ActiveFollowCamera();
        ResetTarget();
    }

}
