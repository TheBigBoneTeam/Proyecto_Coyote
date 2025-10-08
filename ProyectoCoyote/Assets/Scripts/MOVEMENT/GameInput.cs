using UnityEngine;

public class GameInput : MonoBehaviour
{
    [Header("Controles")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode dashKey = KeyCode.LeftControl;

    // Ejes de movimiento
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    
    // Acciones
    public bool JumpPressed { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool DashPressed { get; private set; }


    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        JumpPressed = Input.GetKeyDown(jumpKey);
        SprintHeld = Input.GetKey(sprintKey);
        DashPressed = Input.GetKeyDown(dashKey);
    }

    public Vector2 GetMovementPlayer()
    {
        return new Vector2 (Horizontal, Vertical);
    }
}
