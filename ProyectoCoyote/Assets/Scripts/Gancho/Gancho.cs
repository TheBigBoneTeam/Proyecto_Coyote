using NUnit.Framework;
using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Gancho : MonoBehaviour
{
    [SerializeField] LayerMask targetLayers;
    Transform HookableObjectLocator;
    Transform cam;
    public Transform currentTarget = null;
    [Header("Settings")]
    [SerializeField] bool zeroVert_Look;
    [SerializeField] float noticeZone = 10;
    [SerializeField] float lookAtSmoothing = 2;
    [Tooltip("Angle_Degree")][SerializeField] float maxNoticeAngle = 60;

    Collider[] nearbyTargets;

    PlayerMovement movement;
    CameraController CamControl;
    Transform HookCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CamControl = FindAnyObjectByType<CameraController>();
        HookableObjectLocator = GameObject.Find("HookableObjectLocator").transform;
        movement = FindAnyObjectByType<PlayerMovement>();
        HookCanvas = GameObject.Find("HookCanvas").transform;

        HookCanvas.gameObject.SetActive(false);
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) ActivateTargetHook();
        if (Input.GetKeyDown(KeyCode.X)) 
        {
            currentTarget = FindDirectionalTarget(true);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            currentTarget = FindDirectionalTarget(false);
        }

        if (currentTarget) 
        { 
            LookAtTarget(); 
        }
        
    }

    public void ActivateTargetHook()
    {
        if (currentTarget) // Si ya hay un objeto enganchable, resetear
        {
            ResetTarget();
            return;
        }

        currentTarget = ScanNearBy();
        if (currentTarget != null) 
        {
            // Parar movimiento
            HookCanvas.gameObject.SetActive(true);
            CamControl.ActiveHookCamera();
            Debug.Log("----------Cámara gancho Activada");
        } else ResetTarget();
           
    }
    void ResetTarget()
    {
        // Restaurar movimiento
        HookCanvas.gameObject.SetActive(false);
        currentTarget = null;
        CamControl.ActiveFollowCamera();
        Debug.Log("Volviendo a modo libre");
    }
 
    /*
     * Calcular el objetivo más cercano al objeto fijado en función de si está a la derecha(toRight = true) o a la izquierda(toRight = false)
     */

    private Transform FindDirectionalTarget(bool toRight)
    {
        // Escanear objetivos cercanos
        Collider[] candidates = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
        Transform bestTarget = null;
        float bestAngle = 180f;

        // Si ya hay un objetivo (currentTarget), usa la dirección
        // desde el jugador hacia ese objetivo.
        //Si no hay ninguno, usa la dirección de la cámara.
                Vector3 referenceDir = currentTarget != null
            ? currentTarget.position - transform.position
            : cam.forward;

        referenceDir.y = 0;
        referenceDir.Normalize();

        // Recorre todos los candidatos
        foreach (var col in candidates)
        {
            if (col.transform == currentTarget) continue;

            Vector3 dirToTarget = col.transform.position - transform.position;
            dirToTarget.y = 0;
            dirToTarget.Normalize();

            // Calcula si el objeto está a la derecha o a la izquierda
            float angle = Vector3.SignedAngle(referenceDir, dirToTarget, Vector3.up);

            // Filtra por dirección y elige el más cercano
            if (toRight && angle > 5 && angle < bestAngle)
            {
                bestAngle = angle;
                bestTarget = col.transform;
            }
            else if (!toRight && angle < -5 && Mathf.Abs(angle) < bestAngle)
            {
                bestAngle = Mathf.Abs(angle);
                bestTarget = col.transform;
            }
        }

        return bestTarget;
    }

    private Transform ScanNearBy()
    {
        // Crea una esfera al rededor del personaje con radio en noticeZone.
        // Guarda en un array todos los objetos que coincidan con la target
        // definida en targetLayers.
        nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);

        // Inicializa las variables para encontrar el objetivo m�s cercano.
        float closestAngle = maxNoticeAngle;
        Transform closestTarget = null;

        // Si no hay objetivos cerca, se sale.
        if (nearbyTargets.Length <= 0)
        {
            Debug.Log("No se han encontrado objetos enganchables cerca!");
            return null;
        }

        Debug.Log("Objetos enganchables detectados: ");
        // Recorre todos los objetivos detectados y calcula su direcci�n y 
        // �ngulo desde la c�mara, detecta al m�s cercano.
        for (int i = 0; i < nearbyTargets.Length; i++)
        {
            Debug.Log($"Objeto {i}: {nearbyTargets[i]}");
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
        if (nearbyTargets.Length == 0)
        {
            Debug.Log("No se han encontrado objetos enganchables cerca!");
            return null;
        }


        Debug.Log($"Objeto más cercano: {closestTarget}");

        
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
}
