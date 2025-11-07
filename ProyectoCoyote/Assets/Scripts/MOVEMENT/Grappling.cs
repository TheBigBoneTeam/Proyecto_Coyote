using UnityEngine;
using UnityEngine.InputSystem;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    private GameInput gameInput;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Gancho")]
    public float maxGrappleDistance;
    public float grappleDelayTime;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Vibracion del mando")]
    [Header("Vibración del mando")]
    [Tooltip("Intensidad del motor izquierdo (0-1)")]
    [Range(0, 1)] public float lowVibration = 0.3f;
    [Tooltip("Intensidad del motor derecho (0-1)")]
    [Range(0, 1)] public float highVibration = 0.7f;
    [Tooltip("Duración de la vibración en segundos")]
    public float vibrationDuration = 0.2f;

    private bool grappling;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        gameInput = GetComponent<GameInput>();

        if (Gamepad.current == null)
            Debug.LogWarning("No se detecta ningún mando conectado.");
        else
            Debug.Log($"Mando detectado: {Gamepad.current.displayName}");
    }

    // ANTIGUO UPDATE
    /*
    private void Update()
    {
        if(Input.GetKeyDown(grappleKey))
        {
            StartGrapple();

            if(grapplingCdTimer > 0)
            {
                grapplingCdTimer -= Time.deltaTime;
            }
        }
    }
    */

    // NUEVO UPDATE (para el gameinput)
    private void Update()
    {
        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;

        if (gameInput != null && gameInput.HookAimPressed && !grappling && grapplingCdTimer <= 0)
        {
            StartGrapple();
        }

        if (gameInput != null && gameInput.HookConfirmPressed && grappling)
        {
            ExecuteGrapple();
        }

        if (gameInput != null && gameInput.Hook_TPPressed && grappling)
        {
            MoveTowardHookPoint();
        }
    }

    private void LateUpdate()
    {
        if(grappling)
        {
            lr.SetPosition(0, gunTip.position);
        }
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;
        grappling = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            {
                grapplePoint = hit.point;
                Invoke(nameof(ExecuteGrapple), grappleDelayTime);
            }
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);

        StartCoroutine(VibrateGamepad(lowVibration, highVibration, vibrationDuration));
    }

    private void ExecuteGrapple()
    {
        StartCoroutine(VibrateGamepad(highVibration, highVibration, vibrationDuration * 1.5f));
    }

    private void MoveTowardHookPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, grapplePoint, Time.deltaTime * 10f);
    }

    private void StopGrapple()
    {
        grappling = false;
        grapplingCdTimer = grapplingCd;
        lr.enabled = false;
    }

    // VIBRACION
    private System.Collections.IEnumerator VibrateGamepad(float low, float high, float duration)
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(low, high);
            yield return new WaitForSeconds(duration);
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
}
