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

    [Header("Settings")]
    public float rotationSpeed = 5f;
    public float minDistanceToSwitch = 3f;
    private bool lockedCamera;
    private bool hookedCamera;

    [Header("Trasparencias")]
    private List<MeshRenderer> disabledRenderers = new List<MeshRenderer>();
    // Shader
    public Material transparentMaterial;
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();
    private List<Renderer> currentHits = new List<Renderer>();

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        playerObj = GameObject.Find("Player/Player_02").transform;
        enemyLockOn = GameObject.FindAnyObjectByType<EnemyLockOn>();
        hook = GameObject.FindAnyObjectByType<Gancho>();
    }

    private void LateUpdate()
    {
        lockedCamera = enemyLockOn != null && enemyLockOn.enemyLocked;
        hookedCamera = hook != null && hook.selectingHook;

        if (lockedCamera) HandleTarget(enemyLockOn.currentTarget, lockOnCamera);
        else if (hookedCamera) HandleTarget(hook.currentTarget, hookCamera, hook.lookAtRotationOffset);
        else RotateFreePlayer();

       HandleOcclusion(); // HandleTransparency();

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

    // Maneja la lógica común de lock-on y hook.
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

    private void HandleOcclusion()
    {
        Vector3 origin = cam.transform.position;
        Vector3 target = playerObj.position + Vector3.up * 1.5f;
        Vector3 dir = target - origin;
        float dist = dir.magnitude;

        // Restaurar renderers 
        foreach (MeshRenderer rend in disabledRenderers)
        {
            if (rend != null) rend.enabled = true;
        }
        disabledRenderers.Clear();

        // Raycast hacia el jugador
        RaycastHit[] hits = Physics.RaycastAll(origin, dir.normalized, dist);
        foreach (RaycastHit hit in hits)
        {
            MeshRenderer rend = hit.collider.GetComponent<MeshRenderer>();
            if (rend != null)
            {
                rend.enabled = false; // Desactivar render
                disabledRenderers.Add(rend);
            }
        }
    }

    // Para hacerlo con shaders
    private void HandleTransparency()
    {
        Vector3 origin = cam.transform.position;
        Vector3 target = playerObj.position + Vector3.up * 1.5f;
        Vector3 dir = target - origin;
        float dist = dir.magnitude;

        // Restaurar materiales
        foreach (Renderer rend in currentHits)
        {
            if (rend != null && originalMaterials.ContainsKey(rend))
            {
                rend.material = originalMaterials[rend];
            }
        }
        currentHits.Clear();

        // Raycast hacia el jugador
        RaycastHit[] hits = Physics.RaycastAll(origin, dir.normalized, dist);
        foreach (RaycastHit hit in hits)
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend != null)
            {
                // Guardar material original si no lo tenemos
                if (!originalMaterials.ContainsKey(rend))
                {
                    originalMaterials[rend] = rend.material;
                }

                // Aplicar material transparente
                rend.material = transparentMaterial;

                // Añadir a lista de objetos transparentes este frame
                currentHits.Add(rend);
            }
        }


    }

}