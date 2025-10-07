using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float dashSpeed;
    public float dashSpeedChangeFactor;

    public float groundDrag;    // Deslizamiento

    // Para controlar el deslizamiento y movimiento (EN EL SUELO!)
    [Header("Chequeo de suelo")]
    public float playerHeight;
    public LayerMask groundLayer;
    public bool grounded;

    [Header("Manejo de caída")]
    /*
    0: normal
    1: gravedad estandar
    2: caida rapida
    0.5: caida lenta
    5: caida intensa
     */
    public float gravity;

    [Header("Manejo de rampas")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public float jumpForce;
    public float jumpCooldown;
    public float airSensitity;
    bool readyToJump;

    [Header("Controles")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    public Transform orientation;
    float horizontalInput, verticalInput;
    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    [Header("Animator")]
    public Animator animator;
    Vector2 inputDirection = new Vector2();
    bool isRunning;
    float inputMagnitude;
    float moveX, moveY;

    public enum MovementState
    {
        walking,
        sprinting,
        dashing,
        air
    }

    public bool dashing;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Check del suelo
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        MyInput();
        SpeedControl();
        StateHandler();

        // Manipulacion del deslizamiento
        if (state == MovementState.walking || state == MovementState.sprinting)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
            rb.AddForce(Vector3.down * gravity, ForceMode.Force);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        computeAnimator();

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;

    private void computeAnimator()
    {
        inputDirection.x = horizontalInput;
        inputDirection.y = verticalInput;
        inputMagnitude = inputDirection.magnitude;


        animator.SetFloat("Input", inputMagnitude);
        animator.SetBool("isRunning", isRunning);



        float movement = Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput);
        animator.SetFloat("Horizontal", horizontalInput, 0.2f, Time.deltaTime);
        animator.SetFloat("Vertical", verticalInput, 0.2f, Time.deltaTime);
        animator.SetFloat("Movement", movement);
    }
    private void StateHandler()
    {
        // Modo dash
        if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }
        // Modo correr
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
            isRunning = true;
        }

        // Modo andar
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
            isRunning = false;
        }

        // Modo aereo
        else
        {
            state = MovementState.air;

            if (desiredMoveSpeed < sprintSpeed)
            {
                desiredMoveSpeed = walkSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing) keepMomentum = true;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutines();
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }

    // Codigo para mantener el momentum despues del dash
    private float speedChangeFactor;

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime * boostFactor;
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * 10f, ForceMode.Force);

        // Para rampas
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airSensitity, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        // Control de velocidad en rampa
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }

        // Control de velocidad en suelo o aire
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    /**/
    private void Jump()
    {
        exitingSlope = true;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = true;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;   // Se normaliza la normal (ya que es una direccion)
    }

    /* MOSTRAR POR PANTALLA VELOCIDAD Y ALTURA */
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 30;   // Tamaño de la letra

        // Velocidad horizontal (solo plano XZ)
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float speed = flatVel.magnitude;

        // Altura
        float height = transform.position.y;

        GUI.Label(new Rect(10, 10, 400, 40), "Velocidad: " + speed.ToString("F2") + " m/s");
        GUI.Label(new Rect(10, 50, 400, 40), "Altura: " + height.ToString("F2") + " m");
    }
    /**/
}
