using UnityEngine;
// Movimiento provisional para probar la cámara
public class PlayerMovement_Borrar : MonoBehaviour
{
    // 1. Añadir cámara
    public Transform Camara;
    //
    public float speed = 5f;       

    private CharacterController controller;
    
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical");   
        Vector3 direction = new Vector3(x, 0, z);

        if (direction.magnitude >= 0.1f) 
        {
            // 2. Añadir camara al angulo de giro
            float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg + Camara.eulerAngles.y;
            //

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // 3. Añadir al movimiento la direccion del angulo al que esté mirando
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        

    }
}
