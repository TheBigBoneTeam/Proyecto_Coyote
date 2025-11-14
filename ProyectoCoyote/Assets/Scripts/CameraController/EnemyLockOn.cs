using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

// Clase que se encarga de lockear al enemigo
public class EnemyLockOn : MonoBehaviour
{

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

    private GameObject UIMobile_Combat;
    private GameObject UIMobile_NonCombat;

    // InputSystem
    private GameInput gameInput;
    private bool prevLockPressed = false;

    void Start()
    {
        CamControl = FindAnyObjectByType<CameraController>();
        hook = FindAnyObjectByType<Gancho>();

        lockOnCanvas = GameObject.Find("LockOnCanvas").transform;
        enemyTarget_Locator = GameObject.Find("EnemyTarget_Locator").transform;
        movement = GetComponent<PlayerMovement>();
        defenseAttackUIIndicator = GetComponentInChildren<DefenseAttackUIIndicator>();
        enemyDefenseAttackUIIndicator = FindAnyObjectByType<EnemyDefenseAttackUIIndicator>();

        cam = Camera.main.transform;

        gameInput = GetComponent<GameInput>();
        if (gameInput == null) gameInput = GetComponentInParent<GameInput>();

        lockOnCanvas.gameObject.SetActive(false); // UI de enemigo lockeado

        // Buscar las UIs móviles por nombre en la escena
        UIMobile_Combat = GameObject.Find("MobileUI_Combat");
        UIMobile_NonCombat = GameObject.Find("MobileUI_NonCombat");

        if (UIMobile_Combat == null)
            Debug.LogWarning("[EnemyLockOn] No se encontró el canvas 'MobileUI_Combat' en la escena.");

        if (UIMobile_NonCombat == null)
            Debug.LogWarning("[EnemyLockOn] No se encontró el canvas 'MobileUI_NonCombat' en la escena.");

        // Estado inicial: sin combate
        if (UIMobile_NonCombat != null) UIMobile_NonCombat.SetActive(true);
        if (UIMobile_Combat != null) UIMobile_Combat.SetActive(false);
    }

    void Update()
    {
        // Bloqueo de movimiento cuando hay lock
        movement.lockMovement = enemyLocked;

        bool modoGancho = hook.isHooked || hook.selectingHook;

        // Lectura del input Lock (Q o botón mando)
        bool currentLock = gameInput != null && gameInput.LockPressed;
        if (currentLock && !prevLockPressed && !modoGancho)
        {
            ActivateLockMode();
        }
        prevLockPressed = currentLock;

        if (enemyLocked)
        {
            LookAtTarget();

            // Volver a modo sin lockear si hay un obstáculo
            if (Blocked(GetTargetCenter(currentTarget)))
                ResetTarget();
        }
    }

    // Activar modo lockeado
    public void ActivateLockMode()
    {
        Debug.Log("Activando modo Lock...");

        if (enemyLocked)
        {
            Debug.Log("Ya hay un enemigo lockeado. Reseteando...");
            ResetTarget();
            return;
        }

        if (currentTarget = ScanNearBy())
            FoundTarget();
        else
            ResetTarget();

        Debug.Log("Modo Lock activado");
        AudioManager.Instance.PlaySimpleSound("SFX - Select Hookable Object", false, Vector2.zero, true, true);
    }

    // Se ha encontrado un enemigo válido
    public void FoundTarget()
    {
        if (!currentTarget)
        {
            Debug.Log("Enemigo no válido");
            return;
        }

        lockOnCanvas.gameObject.SetActive(true);

        defenseAttackUIIndicator.setEnemy(currentTarget.GetComponent<AGameCharacter>());
        //if (enemyDefenseAttackUIIndicator != null)
        //    enemyDefenseAttackUIIndicator.setCharacter(currentTarget.GetComponent<AGameCharacter>());

        CamControl.ActiveTargetLookingCamera();
        enemyLocked = true;

        Debug.Log("Enemigo encontrado");
        // Se activa la interfaz de movil de combate
        UIMobile_Combat.SetActive(true);
        UIMobile_NonCombat.SetActive(false);
    }

    // Resetear el lock
    public void ResetTarget()
    {
        lockOnCanvas.gameObject.SetActive(false);
        //defenseAttackUIIndicator.setEnable(false);
        currentTarget = null;
        enemyLocked = false;

        CamControl.ActiveFollowCamera();
        defenseAttackUIIndicator.setEnemy(null);
        //if (enemyDefenseAttackUIIndicator != null)
        //    enemyDefenseAttackUIIndicator.setCharacter(null);

        Debug.Log("Volviendo a modo SIN lockear");
        // Se desactiva la interfaz de movil de combate
        UIMobile_Combat.SetActive(false);
        UIMobile_NonCombat.SetActive(true);
    }

    // Escanear alrededores en busca de enemigos
    private Transform ScanNearBy()
    {
        Debug.Log("Buscando enemigos...");

        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);

        float closestAngle = maxNoticeAngle;
        Transform closestTarget = null;

        if (nearbyTargets.Length <= 0)
        {
            Debug.Log("No se han encontrado enemigos cerca");
            return null;
        }

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

        if (!closestTarget)
        {
            Debug.Log("No se han encontrado enemigos válidos");
            return null;
        }

        float h1 = closestTarget.GetComponent<CapsuleCollider>().height;
        float h2 = closestTarget.localScale.y;
        float h = h1 * h2;
        float half_h = (h / 2) / 2;
        currentYOffset = h - half_h;

        Vector3 tarPos = closestTarget.position + new Vector3(0, currentYOffset, 0);

        if (Blocked(tarPos))
        {
            Debug.Log("Hay algo bloqueando el enemigo");
            return null;
        }

        return closestTarget;
    }

    // Detectar el centro actual del objetivo
    Vector3 GetTargetCenter(Transform target)
    {
        if (target == null) return Vector3.zero;
        CapsuleCollider col = target.GetComponent<CapsuleCollider>();
        if (col == null) return target.position;

        float h = col.height * target.localScale.y;
        float half_h = (h / 2) / 2;
        return target.position + new Vector3(0, h - half_h, 0);
    }

    // Detectar si hay un objeto bloqueando la visión
    bool Blocked(Vector3 t)
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
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

    // Mirar al enemigo
    private void LookAtTarget()
    {
        if (currentTarget == null)
        {
            ActivateLockMode();
            return;
        }

        pos = currentTarget.position;
        lockOnCanvas.position = pos;
        lockOnCanvas.localScale = Vector3.one * ((cam.position - pos).magnitude * UI_Locked_Scale);

        enemyTarget_Locator.position = pos;
    }

    // Dibujar esfera de detección en el editor
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeZone);
    }

    public void resetWhenDie(Transform deadTarget)
    {
        if (currentTarget == deadTarget)
            ResetTarget();
    }
}
