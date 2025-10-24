using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables de movimiento basico
    [Header("Movimiento")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float dashSpeed;
    public float dashSpeedChangeFactor;

    public float groundDrag;    // Deslizamiento
    /*
     cuanto mas valor tenga, menos desliza
     */

    [Header("Chequeo de suelo")]
    public float playerHeight;
    public LayerMask groundLayer;
    public bool grounded;

    [Header("Manejo de ca�da")]
    public float gravity;
    /*
    0: normal
    0.5: caida lenta
    1: gravedad estandar
    2: caida rapida
    5: caida intensa
    */

    [Header("Manejo de rampas")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public float jumpForce;
    public float jumpCooldown;
    public float airSensitity;
    bool readyToJump;
    #endregion

    #region Variables para el dash
    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    [Header("Dash Cooldown")]
    public float dashCd;
    private float dashCdTimer;
    [Header("Dodge Cooldown")]
    public float dodgeCd;
    private float dodgeCdTimer;
    private Vector3 delayedForceToApply;

    public bool dashing;
    #endregion

    #region Sonidos
    [Header("Configuracion de pisadas")]
    [SerializeField] private string walkSoundName = "PERSONAJE - pisadas ANDAR";
    [SerializeField] private string runSoundName = "PERSONAJE - pisadas CORRER";

    [SerializeField] private float stepIntervalWalk = 0.6f;
    [SerializeField] private float stepIntervalRun = 0.35f;

    private float stepTimer;
    private PlayerMovement playerMovement;

    private bool isFootstepPlaying = false;
    #endregion


    #region Variables de control
    public Transform orientation;
    float horizontalInput, verticalInput;
    [HideInInspector] public Vector3 moveDirection;

    Rigidbody rb;

    // Hola GameInput
    [SerializeField] private GameInput gameInput;


    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;

    [Header("Animator")]
    public Animator animator;
    Vector2 inputDirection = new Vector2();
    bool isRunning;
    float inputMagnitude;
    // float moveX, moveY;

    // A�adido Andrea
    [Header("Lock Movement")]
    
    Transform cam;
    [SerializeField] float moveLockedSpeed = 1f;
    float rotationSpeed = 3f;
    public bool lockMovement;
    [SerializeField] EnemyLockOn lockOnSystem;
    Transform enemyTarget => lockOnSystem != null ? lockOnSystem.currentTarget : null;
    [SerializeField] float rotateSpeed = 3f;
    //
    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        dashing,
        air
    }

    //Bools para bloquear acciones mediante animator
    bool canAttack,canMove = false;
    #endregion
    #region Managers
    IGameStateManager gameStateManager;
    IPerfectDodgeManager perfectDodgeManager;
    #endregion
    #region Metodos de Unity

    private void Start()
    {
        canMove = canAttack = true;
        cam = Camera.main.transform;
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        animator = GetComponentInChildren<Animator>();

        gameInput = GetComponentInParent<GameInput>();
        if (gameInput == null)
        {
            Debug.LogError("No se encontr� el GameInput.");
        }
        else
        {
            Debug.Log("GameInput encontrado correctamente: " + gameInput.name);
        }

        gameStateManager = ServiceLocator.Instance.Get<IGameStateManager>();
        perfectDodgeManager = ServiceLocator.Instance.Get<IPerfectDodgeManager>();
    }

    void Update()
    {
        // Check del suelo
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        MyInput();
        SpeedControl();
        StateHandler();
        HandleDashInput();
        HandleFootsteps();

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
        if (lockMovement && enemyTarget != null)
        {
            Vector3 lookDir = enemyTarget.position - transform.position;
            lookDir.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        }
            MovePlayer();
    }
    #endregion

    #region Input
    private void MyInput()
    {
        if (gameInput == null) return;

        horizontalInput = gameInput.Horizontal;
        verticalInput = gameInput.Vertical;

        //  A�adido Andrea
        // tener en cuenta c�mara
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * verticalInput + right * horizontalInput).normalized;
        //

        computeAnimator();

        if(gameInput.JumpPressed && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
       
    }
    #endregion

    #region Animaciones y movimiento
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
        if (gameInput.attackPressed && canAttack)
        {
            string attackName = "";
            if (horizontalInput == 0)
            {
                attackName += "Hit_M_R";
            
            }
            if (horizontalInput > 0)
            {
                attackName += "Hit_R";
            }
            if (horizontalInput < 0)
            {
                attackName += "Hit_L";
                    
            }
            if(gameStateManager.getState() == GameState.SlowDown)
            {
               attackName += "_CRIT";
               perfectDodgeManager.StopSlowdown();
            }
            animator.CrossFade(attackName, .1f);

        }

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
        /*
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
            isRunning = true;
        }
        */
        else if (grounded && gameInput.SprintHeld)
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
        float modeSpeed = lockMovement ? moveLockedSpeed : moveSpeed;
        //// moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //rb.AddForce(moveDirection.normalized * 10f, ForceMode.Force);
        if (canMove)
        {
            // Para rampas
            if (OnSlope() && !exitingSlope)
            {
                rb.AddForce(GetSlopeMoveDirection() * modeSpeed * 20f, ForceMode.Force);

                if (rb.linearVelocity.y > 0)
                {
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }

            else if (grounded)
            {
                rb.AddForce(moveDirection.normalized * modeSpeed * 10f, ForceMode.Force);
            }

            else if (!grounded)
            {
                rb.AddForce(moveDirection.normalized * modeSpeed * 10f * airSensitity, ForceMode.Force);
            }
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

            if (flatVel.magnitude > moveSpeed && !lockMovement)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
            else if(flatVel.magnitude > moveLockedSpeed && lockMovement)
            {
                Vector3 limitedVel = flatVel.normalized * moveLockedSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }


        }

    }
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

    // Manejo del dash
    private void HandleDashInput()
    {
        if (gameInput == null) return;

        dashCdTimer -= Time.deltaTime;
        dodgeCdTimer -= Time.deltaTime;

        if (lockMovement)
        {
            if (gameInput.DashPressed && dodgeCdTimer <= 0f)
            {
                Dodge();
            }
        }
        else
        {
            if (gameInput.DashPressed && dashCdTimer <= 0f)
            {
                Dash();
            }
        }

    }

    private void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        dashing = true;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;
        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);
        Invoke(nameof(ResetDash), dashDuration);
    }
    private void Dodge()
    {
        if (dodgeCdTimer > 0) return;
        else dodgeCdTimer = dodgeCd;

        dashing = true;
            if (horizontalInput == 0)
            {
                animator.CrossFade("Dodge_M", .1f);
            }
            if (horizontalInput > 0)
            {
                animator.CrossFade("Dodge_R", .1f);
            }
            if (horizontalInput < 0)
            {
                animator.CrossFade("Dodge_L", .1f);
            }
        
        Invoke(nameof(ResetDash), dashDuration);
    }
    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        dashing = false;
    }

    /*
    // MOSTRAR POR PANTALLA VELOCIDAD Y ALTURA
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 30;   // Tama�o de la letra

        // Velocidad horizontal (solo plano XZ)
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float speed = flatVel.magnitude;

        // Altura
        float height = transform.position.y;

        GUI.Label(new Rect(10, 10, 400, 40), "Velocidad: " + speed.ToString("F2") + " m/s");
        GUI.Label(new Rect(10, 50, 400, 40), "Altura: " + height.ToString("F2") + " m");
    }
    */

    internal void setCanAttack(bool v)
    {
        canAttack = v;
    }

    internal void setCanMove(bool v)
    {
        canMove = v;
    }
    #endregion

    #region Metodos de sonidos
    private void HandleFootsteps()
    {
        bool isMoving = moveDirection.magnitude > 0.1f && grounded && !dashing;

        if (isMoving)
        {
            float stepInterval = (state == MovementState.sprinting) ? stepIntervalRun : stepIntervalWalk;
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                string soundName = (state == MovementState.sprinting) ? runSoundName : walkSoundName;
                AudioManager.Instance.Play3DSound(soundName, false, transform.position, true, false); // false para reproducir solo 1 vez
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
            isFootstepPlaying = false;
            // Detener sonidos si quieres que desaparezcan inmediatamente
            AudioManager.Instance.StopAllSoundsWithTag(walkSoundName);
            AudioManager.Instance.StopAllSoundsWithTag(runSoundName);
        }
    }

    #endregion
}