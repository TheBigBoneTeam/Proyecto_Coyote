using Unity.VisualScripting;
using UnityEngine;

public class GameInput : MonoBehaviour
{
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

    #region Propiedades publicas
    // Ejes de movimiento
    [field: SerializeField] public float Horizontal { get; private set; }
    [field: SerializeField] public float Vertical { get; private set; }

    // Acciones
   [field:SerializeField] public bool JumpPressed { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool DashPressed { get; private set; }
    public bool attackPressed { get; private set; }
    public bool HookPressed { get; private set; }
    public bool HookSelectPressed { get; private set; }
    public bool LockPressed { get; private set; }

    public bool UpPressed { get; private set; }
    public bool DownPressed { get; private set; }
    public bool LeftPressed { get; private set; }
    public bool RightPressed { get; private set; }
    #endregion

    #region Metodos
    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        JumpPressed = Input.GetKeyDown(jumpKey);
        SprintHeld = Input.GetKey(sprintKey);
        DashPressed = Input.GetKeyDown(dashKey) || Input.GetMouseButtonDown(1);

        attackPressed = Input.GetMouseButtonDown(0);

        HookPressed = Input.GetKeyDown(hookKey) || Input.GetMouseButtonDown(2);
        HookSelectPressed = Input.GetKeyDown(hookSelectKey);
        LockPressed = Input.GetKeyDown(lockKey);

        UpPressed = Input.GetKeyDown(UpKey);
        DownPressed = Input.GetKeyDown(DownKey);
        LeftPressed = Input.GetKeyDown(LeftKey);
        RightPressed = Input.GetKeyDown(RightKey);
    }

    public Vector2 GetMovementPlayer()
    {
        return new Vector2 (Horizontal, Vertical);
    }
    #endregion
}
