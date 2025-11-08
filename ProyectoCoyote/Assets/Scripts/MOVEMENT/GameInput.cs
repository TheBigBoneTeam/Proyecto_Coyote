using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerControls controls;

    // Ejes de movimiento
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    /*
    #region Variables de entrada
    [Header("Controles")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode dashKey = KeyCode.LeftControl;
    public KeyCode hookKey = KeyCode.E;
    public KeyCode hookSelectKey = KeyCode.Mouse0;
    public KeyCode lockKey = KeyCode.Q;

    public KeyCode UpKey = KeyCode.W;
    public KeyCode DownKey = KeyCode.S;
    public KeyCode LeftKey = KeyCode.A;
    public KeyCode RightKey = KeyCode.D;
    #endregion
    */

    // Acciones
    public bool SprintHeld { get; private set; }
    public bool DashPressed { get; private set; }
    public bool LockPressed { get; private set; }
    public bool HookAimPressed { get; private set; }
    public bool HookConfirmPressed { get; private set; }
    public bool HookDisconfirmPressed { get; private set; }
    public bool HookSelectLeftPressed { get; private set; }
    public bool HookSelectRightPressed { get; private set; }
    public bool Hook_TPPressed { get; private set; }
    public bool HookAttractPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool EvadePressed { get; private set; }

    #region Metodos

    private void Awake()
    {
        controls = new PlayerControls();

        // --- Movimiento ---
        controls.Player.Walk.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            Horizontal = input.x;
            Vertical = input.y;
        };
        controls.Player.Walk.canceled += ctx =>
        {
            Horizontal = 0;
            Vertical = 0;
        };

        // --- Sprint (mantener) ---
        controls.Player.Sprint.performed += ctx => SprintHeld = true;
        controls.Player.Sprint.canceled += ctx => SprintHeld = false;

        // --- Dash (pulsaci�n) ---
        controls.Player.Dash.performed += ctx => DashPressed = true;

        // --- Attack (pulsaci�n) ---
        controls.Player.Attack.performed += ctx => AttackPressed = true;

        // --- Evade (pulsaci�n) ---
        controls.Player.Evade.performed += ctx => EvadePressed = true;

        // --- Lock (pulsaci�n �nica) ---
        controls.Player.Lock.performed += ctx => LockPressed = true;

        // --- Gancho (pulsaciones �nicas) ---
        controls.Player.HookAim.performed += ctx => HookAimPressed = true;
        controls.Player.HookConfirm.performed += ctx => HookConfirmPressed = true;
        controls.Player.HookDisconfirm.performed += ctx => HookDisconfirmPressed = true;
        controls.Player.HookSelectLeft.performed += ctx => HookSelectLeftPressed = true;
        controls.Player.HookSelectRight.performed += ctx => HookSelectRightPressed = true;
        controls.Player.Hook_TP.performed += ctx => Hook_TPPressed = true;
        controls.Player.HookAttract.performed += ctx => HookAttractPressed = true;

    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void LateUpdate()
    {
        // Reset autom�tico cada frame (para pulsaci�n �nica)
        DashPressed = false;
        AttackPressed = false;
        EvadePressed = false;
        LockPressed = false;
        HookAimPressed = false;
        HookConfirmPressed = false;
        HookDisconfirmPressed = false;
        HookSelectLeftPressed = false;
        HookSelectRightPressed = false;
        Hook_TPPressed = false;
        HookAttractPressed = false;
    }

    public Vector2 GetMovementPlayer()
    {
        return new Vector2 (Horizontal, Vertical);
    }
    #endregion
}
