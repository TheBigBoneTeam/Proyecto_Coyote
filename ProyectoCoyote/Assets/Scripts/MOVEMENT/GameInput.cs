using UnityEngine;

public class GameInput : MonoBehaviour
{
    #region Variables de entrada
    [Header("Controles")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode dashKey = KeyCode.LeftControl;
    public KeyCode hookKey = KeyCode.E;
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
    }

    public Vector2 GetMovementPlayer()
    {
        return new Vector2 (Horizontal, Vertical);
    }
    #endregion
}
