using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public enum DeviceType { KeyboardMouse, Gamepad, Mobile }
    public DeviceType CurrentDevice { get; private set; } = DeviceType.KeyboardMouse;

    private PlayerControls controls;

    // Movimiento
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    // Acciones
    public bool SprintHeld { get; private set; }
    public bool DashPressed { get; private set; }
    public bool LockPressed { get; private set; }
    public bool HookAimPressed { get; private set; }
    public bool HookConfirmPressed { get; private set; }
    public bool Hook_SelectUp { get; private set; }
    public bool Hook_SelectDown { get; private set; }
    public bool Hook_SelectLeft { get; private set; }
    public bool Hook_SelectRight { get; private set; }
    public bool HookAttractPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool AttackRightPressed { get; private set; }

    public bool EvadePressed { get; private set; }
    public bool Evade_LeftPressed { get; private set; }
    public bool Evade_RightPressed { get; private set; }
    public bool EscapePressed { get; private set; }
    public bool SkipPressed { get; private set; }

    private void Awake()
    {
        // Detectar móvil antes que nada
        if (Application.isMobilePlatform)
        {
            CurrentDevice = DeviceType.Mobile;
        }

        controls = new PlayerControls();

        // MOVIMIENTO
        controls.Player.Walk.performed += ctx =>
        {
            var input = ctx.ReadValue<Vector2>();
            Horizontal = input.x;
            Vertical = input.y;

            DetectDeviceFromContext(ctx);
        };

        controls.Player.Walk.canceled += ctx =>
        {
            Horizontal = 0;
            Vertical = 0;
        };

        // SPRINT
        controls.Player.Sprint.performed += ctx =>
        {
            SprintHeld = true;
            DetectDeviceFromContext(ctx);
        };
        controls.Player.Sprint.canceled += ctx => SprintHeld = false;

        // DASH
        controls.Player.Dash.performed += ctx => { DashPressed = true; DetectDeviceFromContext(ctx); };

        // ATAQUE
        controls.Player.Attack.performed += ctx => { AttackPressed = true; DetectDeviceFromContext(ctx); };
        controls.Player.AttackR.performed += ctx => { AttackRightPressed = true; DetectDeviceFromContext(ctx); };

        controls.Player.Evade.performed += ctx => { EvadePressed = true; DetectDeviceFromContext(ctx); };

        // ESQUIVES LATERALES
        /*
        controls.Player.Evade_Left.performed += ctx => { Evade_LeftPressed = true; DetectDeviceFromContext(ctx); };
        controls.Player.Evade_Right.performed += ctx => { Evade_RightPressed = true; DetectDeviceFromContext(ctx); };
        */

        controls.Player.Evade_Left.started += ctx => { Evade_LeftPressed = true; };
        controls.Player.Evade_Left.canceled += ctx => { Evade_LeftPressed = false; };

        controls.Player.Evade_Right.started += ctx => { Evade_RightPressed = true; };
        controls.Player.Evade_Right.canceled += ctx => { Evade_RightPressed = false; };


        // LOCKEO
        controls.Player.Lock.performed += ctx => { LockPressed = true; DetectDeviceFromContext(ctx); };

        // GANCHO
        controls.Player.HookAim.performed += ctx => { HookAimPressed = true; DetectDeviceFromContext(ctx); };
        controls.Player.HookConfirm.performed += ctx => { HookConfirmPressed = true; DetectDeviceFromContext(ctx); };
        controls.Player.HookDisconfirm.performed += ctx => { Hook_SelectDown = true; DetectDeviceFromContext(ctx); };
        controls.Player.HookSelectLeft.performed += ctx => { Hook_SelectLeft = true; DetectDeviceFromContext(ctx); };
        controls.Player.HookSelectRight.performed += ctx => { Hook_SelectRight = true; DetectDeviceFromContext(ctx); };
        controls.Player.Hook_TP.performed += ctx => { Hook_SelectUp = true; DetectDeviceFromContext(ctx); };
        controls.Player.HookAttract.performed += ctx => { HookAttractPressed = true; DetectDeviceFromContext(ctx); };

        // MENU
        controls.Player.Escape.performed += ctx => { EscapePressed = true; DetectDeviceFromContext(ctx); };

        // SALTAR DIALOGOS
        controls.Player.Skip.performed += ctx => { SkipPressed = true; DetectDeviceFromContext(ctx); };
        controls.Player.Skip.canceled += ctx => { SkipPressed = false; };
    }

    private void DetectDeviceFromContext(InputAction.CallbackContext ctx)
    {
        if (CurrentDevice == DeviceType.Mobile)
            return; // móvil ya fijado

        var device = ctx.control.device;

        if (device is Gamepad)
            CurrentDevice = DeviceType.Gamepad;
        else
            CurrentDevice = DeviceType.KeyboardMouse;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    public void ResetOneFrameInputs()
    {
        DashPressed = false;
        EvadePressed = false;
                AttackRightPressed = false;
        Evade_LeftPressed = false;
        Evade_RightPressed = false;
        AttackPressed = false;
        //LockPressed = false;
        HookAimPressed = false;
        HookConfirmPressed = false;
        Hook_SelectUp = false;
        Hook_SelectDown = false;
        Hook_SelectLeft = false;
        Hook_SelectRight = false;
        HookAttractPressed = false;
        EscapePressed = false;
    }


    private void LateUpdate()
    {
      // // Reset pulsaciones únicas
      // AttackRightPressed = false;
      //  DashPressed = false;

      ////  COMBATE
      // AttackPressed = false;
      //  EvadePressed = false;
      //  Evade_LeftPressed = false;
      //  Evade_RightPressed = false;
        LockPressed = false;

      // // GANCHO
      // HookAimPressed = false;
      //  HookConfirmPressed = false;
      //  Hook_SelectDown = false;
      //  Hook_SelectLeft = false;
      //  Hook_SelectRight = false;
      //  Hook_SelectUp = false;
      //  HookAttractPressed = false;

      ////  MENU
      // EscapePressed = false;
    }


    public Vector2 GetMovementPlayer()
    {
        return new Vector2(Horizontal, Vertical);
    }
}
