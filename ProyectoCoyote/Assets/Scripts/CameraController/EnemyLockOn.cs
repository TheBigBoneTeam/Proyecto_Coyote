using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

// Clase que se encarga de lockear al enemigo
public class EnemyLockOn : MonoBehaviour
{
    private GameInput gameInput;

    [SerializeField] LayerMask targetLayers;
    Transform enemyTarget_Locator;
    public Transform currentTarget = null;
    
    CameraController CamControl;

    [Header("Settings")]
    [SerializeField] bool zeroVert_Look;
    [SerializeField] float noticeZone = 10;
    [SerializeField] float lookAtSmoothing = 2;
    [Tooltip("Angle_Degree")][SerializeField] float maxNoticeAngle = 180;
    [SerializeField] float UI_Locked_Scale = 0.1f;

    Transform cam;
    public bool enemyLocked;
    float currentYOffset;
    Vector3 pos;

    [SerializeField] Transform lockOnCanvas;
    PlayerMovement movement;
    [SerializeField] DefenseAttackUIIndicator defenseAttackUIIndicator;
    [SerializeField] EnemyDefenseAttackUIIndicator enemyDefenseAttackUIIndicator;
    Gancho hook;

    void Start()
    {
        CamControl = FindAnyObjectByType<CameraController>();
        hook = FindAnyObjectByType<Gancho>();

        gameInput = FindAnyObjectByType<GameInput>();
        lockOnCanvas = GameObject.Find("LockOnCanvas").transform;
        enemyTarget_Locator = GameObject.Find("EnemyTarget_Locator").transform;
        movement = GetComponent<PlayerMovement>();
        defenseAttackUIIndicator = GetComponentInChildren<DefenseAttackUIIndicator>();
        enemyDefenseAttackUIIndicator = FindAnyObjectByType<EnemyDefenseAttackUIIndicator>();

        cam = Camera.main.transform;
       
        lockOnCanvas.gameObject.SetActive(false); // UI de enemigo lockeado
    }

    // Update is called once per frame
    void Update()
    {
        // Indicar al resto de scripts cu�ndo est� el enemigo lockeado
       
        movement.lockMovement = enemyLocked;

        bool modoGancho = hook.isHooked || hook.selectingHook;
        //// Input System
        if (gameInput.LockPressed && !modoGancho)
        {
            ActivateLockMode();
        }
        //

        if (enemyLocked)
        {
            // if (!TargetOnRange()) ResetTarget();

            LookAtTarget();
            // Volver a modo sin lockear si hay un obst�culo
            if (Blocked(GetTargetCenter(currentTarget))) ResetTarget();
            
        }
    }

    // Activar modo lockeado
    public void ActivateLockMode() 
    {
        Debug.Log("Activando modo Lock....");
        if (enemyLocked) // Si ya hay un enemigo, resetear
        {
            Debug.Log("YA HAY IN ENEMIGO, Reseteando...");
            ResetTarget();
            return;
        }
        

        if (currentTarget = ScanNearBy()) FoundTarget(); else ResetTarget();
        Debug.Log("Modo Lock");
    }


    // Esta funci�n indica si se ha encontrado un enemigo
    public void FoundTarget()
    {
        if (!currentTarget) 
        {
            Debug.Log("Enemigo no válido");
            return; 
        }

        lockOnCanvas.gameObject.SetActive(true);
        
        
        defenseAttackUIIndicator.setEnemy(currentTarget.GetComponent<AGameCharacter>());
        if (enemyDefenseAttackUIIndicator != null)
        {
            enemyDefenseAttackUIIndicator.setCharacter(currentTarget.GetComponent<AGameCharacter>());
        }

        CamControl.ActiveTargetLookingCamera();

        enemyLocked = true;

        Debug.Log("Enemigo encontrado");
    }


    // Esta funci�n vuelve al modo sin lockear reseteando todos los elementos del script
    void ResetTarget()
    {
        lockOnCanvas.gameObject.SetActive(false);
        defenseAttackUIIndicator.setEnable(false);
        currentTarget = null;
        enemyLocked = false;
        CamControl.ActiveFollowCamera();
        defenseAttackUIIndicator.setEnemy(null);
        enemyDefenseAttackUIIndicator.setCharacter(null);
        

        Debug.Log("Volviendo a modo SIN lockear");
    }


    // Escanear alrededores en busca de un enemigo:
    private Transform ScanNearBy()
    {
        Debug.Log("Buscando enemigos...");
        // Crea una esfera al rededor del personaje con radio en noticeZone.
        // Guarda en un array todos los objetos que coincidan con la target
        // definida en targetLayers.
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);

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

            Vector3 dir = nearbyTargets[i].transform.position - cam.position;
            dir.y = 0;
            float _angle = Vector3.Angle(cam.forward, dir);

            Debug.Log($"Distancia al objeto {nearbyTargets[i]}: {_angle}");

            if (_angle < closestAngle)
            {
                closestTarget = nearbyTargets[i].transform;
                closestAngle = _angle;
            }
        }

        // Si no hay objetivos cerca, se sale.
        if (!closestTarget)
        {
            Debug.Log("No se han encontrado enemigos válidos!");
            return null;
        }

        // Calcula la altura del objetivo para ajustar la mirada al centro 
        // del enemigo
        float h1 = closestTarget.GetComponent<CapsuleCollider>().height;
        float h2 = closestTarget.localScale.y;
        float h = h1 * h2;
        float half_h = (h / 2) / 2;
        currentYOffset = h - half_h;

        // Calcula la posici�n final del objetivo
        Vector3 tarPos = closestTarget.position + new Vector3(0, currentYOffset, 0);


        // Si hay algun elemento de la escena bloqueando la visi�n del jugador, se sale.
        if (Blocked(tarPos))
        {
            Debug.Log("Hay algo bloqueando el enemigo");
            return null;
        }
        
        // Devuelve el enemigo v�lido
        return closestTarget;
    }

    // Detectar el centro actual de la target
    Vector3 GetTargetCenter(Transform target)
    {
        if (target == null) { return Vector3.zero; }
        CapsuleCollider col = target.GetComponent<CapsuleCollider>();
        if (col == null) return target.position;

        float h = col.height * target.localScale.y;
        float half_h = (h / 2) / 2;
        return target.position + new Vector3(0, h - half_h, 0);
    }

    // Detectar si hay un objeto bloqueando las escena
    bool Blocked(Vector3 t)
    {
        RaycastHit hit;
        Vector3 origin = cam.position;
        if (Physics.Linecast(origin, t, out hit))
        {
            if (!hit.transform.Equals(currentTarget))
            {
                Debug.Log($"Hay algo bloqueando al enemigo: {hit.transform}");
                return true;
            }
        }
        return false;
    }


    // Calcula si el enemigo est� en rango
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
            ActivateLockMode();
            return;
        }

        // Actualiza la posici�n del canvas lockOn en funci�n de la c�mara
        pos = currentTarget.position + new Vector3(0, 0 , 0);
        lockOnCanvas.position = pos;
        lockOnCanvas.localScale = Vector3.one * ((cam.position - pos).magnitude * UI_Locked_Scale);

        // Actaliza la posici�n del localizador del enemigo
        enemyTarget_Locator.position = pos;

        //// Gira al personaje hacia el enemigo
        //Vector3 dir = currentTarget.position - transform.position;
        //dir.y = 0;
        //Quaternion rot = Quaternion.LookRotation(dir);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * lookAtSmoothing);
    
    }

    // Esfera al rededor del personaje
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeZone);
    }
}
