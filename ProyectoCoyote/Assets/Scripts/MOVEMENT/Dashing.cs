using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation; // hacia donde se mueve el jugador
    private Rigidbody rb;
    private PlayerMovement pm;
    private GameInput gameInput;

    [Header("Dashing")]
    public float dashForce = 20f;
    public float dashUpwardForce = 2f;
    public float dashDuration = 0.2f;

    [Header("Cooldown")]
    public float dashCd = 1f;
    private float dashCdTimer;

    private Vector3 delayedForceToApply;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        gameInput = GetComponent<GameInput>();
        if (gameInput == null)
            gameInput = GetComponentInParent<GameInput>();
    }

    private void Update()
    {
        // Pulsar dash
        if (gameInput != null && gameInput.DashPressed)
        {
            TryDash();
        }

        // Cooldown
        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    private void TryDash()
    {
        if (dashCdTimer > 0) return;

        dashCdTimer = dashCd;
        Dash();
    }

    private void Dash()
    {
        pm.dashing = true;

        // Dirección de movimiento del joystick
        Vector3 moveDir = orientation.forward * gameInput.Vertical + orientation.right * gameInput.Horizontal;

        // Si no hay input, dash hacia donde mira el jugador
        if (moveDir.magnitude < 0.1f)
            moveDir = orientation.forward;

        moveDir.Normalize();

        Vector3 forceToApply = moveDir * dashForce + Vector3.up * dashUpwardForce;
        delayedForceToApply = forceToApply;

        Invoke(nameof(DelayedDashForce), 0.025f);
        Invoke(nameof(ResetDash), dashDuration);
    }

    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;
    }
}
