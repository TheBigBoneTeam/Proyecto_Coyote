using NUnit.Framework;
using System;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField] float maxNoticeZone = 100;
    [SerializeField] float minNoticeZone = 10;
    [SerializeField] float lookAtSmoothing;
    [SerializeField] public Vector3 lookAtRotationOffset = new Vector3(0, 0, 0);
    [Tooltip("Angle_Degree")][SerializeField] float maxNoticeAngle = 120;
    [SerializeField] int cooldown = 5;
    private TextMeshProUGUI _cooldownUIText;
    private bool _canUseHook = false;

    [Header("When selected")]
    [SerializeField] float OffsetFinalPos = 0;

    [Header("Hook Attack Timing Window")]
    [SerializeField] float attackWindowDuration = 1; // Duración de la ventana de tiempo en segundos
    [SerializeField] float attackWindowStartDelay = 0.1f; // Tiempo después de iniciar retracción antes de abrir la ventana

    private bool isInAttackWindow = false;
    private Coroutine attackWindowCoroutine;

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
    [SerializeField] GameObject navMesh;
    private bool hookAttackBuffer = false;
    private bool canAttack = false;
    private bool isRetracting = false; // Flag para saber si el gancho está retrayendo


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
        _hookImageUI = HookCanvas.Find("HookImage").GetComponent<Image>();
        _cooldownUIText = HookCanvas.Find("CooldownText").GetComponent<TextMeshProUGUI>();
        visualHook = FindAnyObjectByType<VisualHook>();

        lockOn = FindAnyObjectByType<EnemyLockOn>();
        player = GameObject.Find("Player");
        hookController = FindAnyObjectByType<HookController>();
        // Obsever prueba
        // hookObserver = FindAnyObjectByType<HookObserver>();
        // hookObserver.Configure(hookController);

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
            AudioManager.Instance.PlaySimpleSound("SFX - Select Hookable Object", false, Vector2.zero, true, false);
        }


        HookLogic();
        if (currentTarget)
        {
            LookAtTarget();

            if (!isRetracting && Blocked(currentTarget.position, currentTarget))
                ResetTarget();
        }

        if (gameInput.AttackPressed && isInAttackWindow)
        {
            hookAttackBuffer = true;
            Debug.Log("Input de ataque detectado");
        }
    }
    public void HookLogic()
    {

        // Navegación por los objetos enganchables
        if (selectingHook && !isHooked)
        {
            if (gameInput.Hook_SelectUp)
            {
                currentTarget = FindDirectionalTarget(false, true);
                // SFX Seleccionar objetivo arriba
                AudioManager.Instance.PlaySimpleSound("SFX - Select Hookable Object", false, Vector2.zero, true, false);
            }
            else if (gameInput.Hook_SelectDown)
            {
                currentTarget = FindDirectionalTarget(false, false);
                // SFX Seleccionar objetivo abajo
                AudioManager.Instance.PlaySimpleSound("SFX - Select Hookable Object", false, Vector2.zero, true, false);
            }
            else if (gameInput.Hook_SelectRight)
            {
                currentTarget = FindDirectionalTarget(true, false);
                // SFX Seleccionar objetivo derecha
                AudioManager.Instance.PlaySimpleSound("SFX - Select Hookable Object", false, Vector2.zero, true, false);
            }
            else if (gameInput.Hook_SelectLeft)
            {
                currentTarget = FindDirectionalTarget(false, false);
                // SFX Seleccionar objetivo izquierda
                AudioManager.Instance.PlaySimpleSound("SFX - Select Hookable Object", false, Vector2.zero, true, false);
            }
        }
        // Selecionar objeto 
        if (gameInput.HookConfirmPressed && selectingHook)
        {
            SelectTarget();
            movement.animator.CrossFade("Grapple_03", 0.2f);
            // SFX Lanzar gancho
            AudioManager.Instance.PlaySimpleSound("SFX - Releasing Hook", false, Vector2.zero, true, false);
            AudioManager.Instance.PlaySimpleSound("SFX - Revolver girando", false, Vector2.zero, true, false);
        }

        if (isHooked && !selectingHook)
        {
            if (gameInput.HookAttractPressed)
            {
                AtractTarget();
                StartCoroutine(Cooldown());
                hookController.HookUsed();
                // SFX CABLE
                AudioManager.Instance.PlaySimpleSound("SFX - Cable", false, Vector2.zero, true, false);

            }
            else if (gameInput.Hook_SelectUp)
            {
                GoToTarget();
                StartCoroutine(Cooldown());
                hookController.HookUsed();
                // SFX CABLE
                AudioManager.Instance.PlaySimpleSound("SFX - Cable", false, Vector2.zero, true, false);
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

            // Si se detecta el modo movil, se activa la interfaz del gancho
            if (MobileUIManager.Instance != null)
            {
                MobileUIManager.Instance.SetHookUI();
            }
        }

    }
    public void ResetTarget(bool skipAnimation = false)
    {
        if (!skipAnimation) movement.animator.CrossFade("Idle_01", 0.2f);
        movement.stopHookMode();
        _hookImageUI.gameObject.SetActive(false);
        visualHook.RetractHook();
        EnableAllCollisions(currentTarget);
        EnableAllCollisions(transform);
        currentTarget = null;
        selectingHook = false;
        isHooked = false;
        isRetracting = false;
        _hookImageUI.color = Color.white;

        StopAttackWindow();

        if (!lockOn.enemyLocked) CamControl.ActiveFollowCamera();
        Debug.Log("Se ha desactivado el gancho. Volviendo a modo libre");

        // Si se detecta el modo movil, se desactiva la interfaz del gancho
        if (MobileUIManager.Instance != null)
        {
            MobileUIManager.Instance.SetNonCombatUI();
        }
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
        float bestScore = Mathf.Infinity;

        foreach (var c in candidates)
        {
            Transform candidate = c.transform;
            if (candidate == currentTarget) continue;

            Vector3 offset = candidate.position - currentTarget.position;
            float distance = offset.magnitude;

            Vector3 rightDir = cam.right;
            float dotRight = Vector3.Dot(offset.normalized, rightDir);
            float verticalOffset = offset.y;

            bool isValid = false;

            // Horizontal 
            if (toRight && dotRight > 0.1f) isValid = true;
            if (!toRight && dotRight < -0.1f) isValid = true;

            // Vertical
            if (toUp && verticalOffset > 0.5f) isValid = true;
            if (!toUp && verticalOffset < -0.5f) isValid = true;

            if (!isValid) continue;
            if (Blocked(candidate.position, candidate.transform)) continue;

            float angle = Vector3.Angle(cam.forward, offset);
            float score = distance + angle * 0.1f;

            if (score < bestScore)
            {
                bestScore = score;
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

        // Inicializa las variables para encontrar el objetivo m s cercano.
        float closestAngle = maxNoticeAngle;
        Transform closestTarget = null;

        // Si no hay objetivos cerca, se sale.
        if (nearbyTargets.Length <= 0)
        {
            Debug.Log("No se han encontrado enemigos cerca!");
            return null;
        }


        // Recorre todos los objetivos detectados y calcula su direcci n y 
        //  ngulo desde la c mara, detecta al m s cercano.
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
            Debug.Log("No se han encontrado objetos válidos");
            return null;
        }

        // Si hay algun elemento de la escena bloqueando la visi n del jugador, se sale.
        if (Blocked(closestTarget.position, closestTarget))
        {
            Debug.Log("Hay algo bloqueando el objeto");
            return null;
        }

        // Devuelve el enemigo v lido
        return closestTarget;
    }

    // Detectar si hay un objeto bloqueando las escena
    bool Blocked(Vector3 targetPosition, Transform target)
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        if (Physics.Linecast(origin, targetPosition, out hit))
        {
            if (!hit.transform.Equals(target) && !hit.transform.Equals(transform))
            {
                Debug.Log($"Hay algo bloqueando al objeto: {hit.transform}");
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

        // Actaliza la posici n del localizador del target
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

            

            selectingHook = false;
            _hookImageUI.color = Color.red;
            visualHook.ThrowHook(currentTarget);
        }
    }
    private void GoToTarget()
    {

        DisableCollisions(transform);
        if (currentTarget == null) return;
        canAttack = true;
        isRetracting = true;
        //// Dirección desde el objeto hacia la cámara
        //Vector3 directionToCamera = (cam.transform.position - currentTarget.position).normalized;
        //// POSICIÓN FINAL
        //Vector3 targetPosition = currentTarget.position + directionToCamera * OffsetFinalPos;

        //// Mover el Rigidbody del jugador
        //var rb = player.GetComponent<Rigidbody>();
        //rb.MovePosition(targetPosition);


        visualHook.RetractHookGoToTarget(OffsetFinalPos);
        movement.animator.CrossFade("Grapple_04", 0.2f);
        StartAttackWindow();

    }

    private void AtractTarget()
    {

        if (hookableObject.canBeHooked)
        {
            DisableCollisions (currentTarget);
            canAttack = true;
            isRetracting = true;
            visualHook.RetractHookAtractTarget(OffsetFinalPos);
            //Vector3 directionToCamera = (cam.transform.position - currentTarget.position).normalized;
            //Vector3 targetPosition = cam.transform.position + directionToCamera * -OffsetFinalPos;
            //currentTarget.position = targetPosition;
            movement.animator.CrossFade("Grapple_04", 0.2f);
            StartAttackWindow();

        }
        else
        {
            CamControl.StartShake();
            ResetTarget();
        }




    }

    #endregion
    #region Attack Window System
    private void StartAttackWindow()
    {
        // Detener corrutina anterior si existe
        if (attackWindowCoroutine != null)
        {
            StopCoroutine(attackWindowCoroutine);
        }

        attackWindowCoroutine = StartCoroutine(AttackWindowCoroutine());
    }

    private void StopAttackWindow()
    {
        isInAttackWindow = false;
        hookAttackBuffer = false;

        if (attackWindowCoroutine != null)
        {
            StopCoroutine(attackWindowCoroutine);
            attackWindowCoroutine = null;
        }
    }

    private IEnumerator AttackWindowCoroutine()
    {
        // Esperar el delay inicial
        yield return new WaitForSeconds(attackWindowStartDelay);

        // Abrir la ventana de ataque
        isInAttackWindow = true;
        Debug.Log("¡VENTANA DE ATAQUE ABIERTA!");

        // Opcional: Feedback visual
        // _hookImageUI.color = Color.yellow; // Indicador visual

        // Mantener la ventana abierta durante la duración especificada
        yield return new WaitForSeconds(attackWindowDuration);

        // Cerrar la ventana
        isInAttackWindow = false;
        Debug.Log("Ventana de ataque cerrada");

        attackWindowCoroutine = null;
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

    #region Collision Management
    public void DisableCollisions(Transform target)
    {
        if (target == null) return;

        Enemy enemy = target.gameObject.GetComponent<Enemy>();
        Player player = target.GetComponent<Player>();

        if (enemy == null && player == null) return;

        // Desactivar Collider
        Collider col = target.gameObject.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Si es enemigo
        if (enemy)
        {
            // Desactivar comportamiento del enemigo
            EnemyAssetBehaviourRunner behaviorRunner = target.gameObject.GetComponent<EnemyAssetBehaviourRunner>();
            if (behaviorRunner != null)
            {
                behaviorRunner.enabled = false;
            }

            // Desactivar NavMeshAgent
            NavMeshAgent agent = target.gameObject.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
            }
        }

        // Si es jugador
        if (player)
        {
            NavMeshObstacle obstacle = target.gameObject.GetComponent<NavMeshObstacle>();
            if (obstacle != null)
            {
                obstacle.enabled = false;
            }

        }

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.useGravity = false;
            rb.isKinematic = true; // Hacer kinematic para evitar interferencias físicas
        }
    }

    public void EnableAllCollisions(Transform target)
    {
        if (target == null) return;

        Enemy enemy = target.gameObject.GetComponent<Enemy>();
        Player player = target.GetComponent<Player>();

        if (enemy == null && player == null) return;

        // Reactivar Collider
        Collider col = target.gameObject.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        // Si es enemigo
        if (enemy)
        {
            NavMeshAgent agent = target.gameObject.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = true;
            }

            EnemyAssetBehaviourRunner behaviorRunner = target.gameObject.GetComponent<EnemyAssetBehaviourRunner>();
            if (behaviorRunner != null)
            {
                behaviorRunner.enabled = true;
            }
        }

        // Si es jugador
        if (player)
        {
            NavMeshObstacle obstacle = target.gameObject.GetComponent<NavMeshObstacle>();
            if (obstacle != null)
            {
                obstacle.enabled = true;
            }
        }

       
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.useGravity = true;
            rb.isKinematic = false; 
        }
    }
    #endregion

    public void WaitForHookFinish()
    {
        currentTarget.gameObject.GetComponent<Collider>().enabled = true;
        

        Debug.Log("Ha llegado a su destino");
        if (currentTarget.gameObject.GetComponent<Enemy>())
        {
            Debug.Log("Es enemigo");
            lockOn.currentTarget = currentTarget;
            lockOn.FoundTarget();
        }
        canAttack = false;
        isRetracting = false;

        if (!lockOn.enemyLocked) CamControl.ActiveFollowCamera();
        HookAttack();
        ResetTarget();
        
    }

    public void HookAttack()
    {
        if (hookAttackBuffer)
        {
            movement.animator.CrossFade("Hit_Gancho", .1f);
            Debug.Log("Gancho patá");
        }
        else
        {
            Debug.Log("No se presionó ataque durante la ventana de tiempo");
        }

        hookAttackBuffer = false;
        StopAttackWindow();
    }



}
