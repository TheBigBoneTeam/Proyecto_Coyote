using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;

// Clase que se encarga de lockear al enemigo
public class EnemyLockOn : MonoBehaviour
{
    Transform currentTarget;
    // Animator anim;

    [SerializeField] LayerMask targetLayers;
    [SerializeField] Transform enemyTarget_Locator;

    [Tooltip("Cambiar entre Cámaras")]
    [SerializeField] Animator cinemachineAnimator;

    [Header("Settings")]
    [SerializeField] bool zeroVert_Look;
    [SerializeField] float noticeZone = 10;
    [SerializeField] float lookAtSmoothing = 2;
    [Tooltip("Angle_Degree")][SerializeField] float maxNoticeAngle = 60;
    [SerializeField] float crossHair_Scale = 0.1f;

    Transform cam;
    public bool enemyLocked;
    float currentYOffset;
    Vector3 pos;

    [SerializeField] CameraFollow camFollow;
    [SerializeField] Transform lockOnCanvas;
    PlayerMovement_Borrar movement; // Editar con movimiento definitivo

    void Start()
    {
        movement = GetComponent<PlayerMovement_Borrar>(); // Modificar
        // anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        lockOnCanvas.gameObject.SetActive(false); // UI de enemigo lockeado

    }

    // Update is called once per frame
    void Update()
    {
        // Indicar al resto de scripts cuándo está el enemigo lockeado
        camFollow.lockedTarget = enemyLocked; 
        movement.lockMovement = enemyLocked;
        //// Input System
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateLockMode();
        }
        //

        if (enemyLocked)
        {
            if (!TargetOnRange()) ResetTarget();
            LookAtTarget();
        }
    }

    // Activar modo lockeado
    void ActivateLockMode() 
    {
        if (currentTarget) // Si ya hay un enemigo, resetear
        {
            ResetTarget();
            return;
        }

        if (currentTarget = ScanNearBy()) FoundTarget(); else ResetTarget();
    }


    // Esta función indica si se ha encontrado un enemigo
    void FoundTarget()
    {
        lockOnCanvas.gameObject.SetActive(true);
        // anim.SetLayerWeight(1, 1);
        cinemachineAnimator.Play("TargetCamera");
        enemyLocked = true;
        Console.WriteLine("Enemigo encontrado");
    }


    // Esta función vuelve al modo sin lockear reseteando todos los elementos del script
    void ResetTarget()
    {
        lockOnCanvas.gameObject.SetActive(false);
        currentTarget = null;
        enemyLocked = false;
        // anim.SetLayerWeight(1, 0);
        cinemachineAnimator.Play("FollowCamera");
        Console.WriteLine("Vover a modo SIN lockear");
    }


    // Escanear alrededores en busca de un enemigo:
    private Transform ScanNearBy()
    {
        // Crea una esfera al rededor del personaje con radio en noticeZone.
        // Guarda en un array todos los objetos que coincidan con la target
        // definida en targetLayers.
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);

        // Inicializa las variables para encontrar el objetivo más cercano.
        float closestAngle = maxNoticeAngle;
        Transform closestTarget = null;

        // Si no hay objetivos cerca, se sale.
        if (nearbyTargets.Length <= 0) 
        {
            Console.WriteLine("No se han encontrado enemigos cerca!");
            return null;
        }


        // Recorre todos los objetivos detectados y calcula su dirección y 
        // ángulo desde la cámara, detecta al más cercano.
        for (int i = 0; i < nearbyTargets.Length; i++)
        {
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
            Console.WriteLine("No se han encontrado enemigos cerca!");
            return null;
        }

        // Calcula la altura del objetivo para ajustar la mirada al centro 
        // del enemigo
        float h1 = closestTarget.GetComponent<CapsuleCollider>().height;
        float h2 = closestTarget.localScale.y;
        float h = h1 * h2;
        float half_h = (h / 2) / 2;
        currentYOffset = h - half_h;

        // Calcula la posición final del objetivo
        Vector3 tarPos = closestTarget.position + new Vector3(0, currentYOffset, 0);


        // Si no hay objetivos cerca, se sale.
        if (Blocked(tarPos))
        {
            Console.WriteLine("No se han encontrado enemigos cerca!");
            return null;
        }

        // Devuelve el enemigo válido
        return closestTarget;
    }


    // Detectar si hay un objeto bloqueando las escena
    bool Blocked(Vector3 t)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up * 0.5f, t, out hit))
        {
            if (!hit.transform.CompareTag("Enemy"))
            {
                Console.WriteLine("Hay algo bloqueando al enemigo");
                return true;
            }
        }
        return false;
    }


    // Calcula si el enemigo está en rango
    bool TargetOnRange()
    {
        float dis = (transform.position - pos).magnitude;
        if (dis / 2 > noticeZone) return false; else return true;
    }


    // Mirar al enemigo
    private void LookAtTarget()
    {
        // Si desaparece el enemigo al que estamos mirando, reasignar enemigo
        if (currentTarget == null)
        {
            if (currentTarget = ScanNearBy()) FoundTarget(); else ResetTarget();
            return;
        }

        // Actualiza la posición del canvas lockOn en función de la cámara
        pos = currentTarget.position + new Vector3(0, 0 , 0);
        lockOnCanvas.position = pos;
        lockOnCanvas.localScale = Vector3.one * ((cam.position - pos).magnitude * crossHair_Scale);

        // Actaliza la posición del localizador del enemigo
        enemyTarget_Locator.position = pos;

        // Gira al personaje hacia el enemigo
        Vector3 dir = currentTarget.position - transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * lookAtSmoothing);
    }

    // Esfera al rededor del personaje
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeZone);
    }
}
