using UnityEngine;
// Movimiento provisional para probar la cámara
public class PlayerMovement_Borrar : MonoBehaviour
{
    CharacterController controller;
    //Animator anim;
    Transform cam;

    float speedSmoothVelocity;
    float speedSmoothTime;
    float currentSpeed;
    float velocityY;
    Vector3 moveInput;
    Vector3 dir;

    [Header("Settings")]
    [SerializeField] float gravity = 25f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float moveLockedSpeed = 1f;
    [SerializeField] float rotateSpeed = 3f;

    public bool lockMovement;


    void Start()
    {
        //anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        GetInput();
        PlayerMovement();
        // El personaje solo podrá rotar si no esta lockeado
        if (!lockMovement) PlayerRotation();
    }

    private void GetInput()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // tener en cuenta cámara
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        dir = (forward * moveInput.y + right * moveInput.x).normalized;
    }

    private void PlayerMovement()
    {
        // TEner en cuenta modo lockeado
        if (lockMovement)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, moveLockedSpeed, ref speedSmoothVelocity, speedSmoothTime * Time.deltaTime);
        }
        else 
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, moveSpeed, ref speedSmoothVelocity, speedSmoothTime * Time.deltaTime);
        }
        if (velocityY > -10) velocityY -= Time.deltaTime * gravity;
        Vector3 velocity = (dir * currentSpeed) + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

       // anim.SetFloat("Movement", dir.magnitude, 0.1f, Time.deltaTime);
      //  anim.SetFloat("Horizontal", moveInput.x, 0.1f, Time.deltaTime);
       // anim.SetFloat("Vertical", moveInput.y, 0.1f, Time.deltaTime);
    }

    private void PlayerRotation()
    {
        if (dir.magnitude == 0) return;
        Vector3 rotDir = new Vector3(dir.x, dir.y, dir.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), Time.deltaTime * rotateSpeed);
    }
}
