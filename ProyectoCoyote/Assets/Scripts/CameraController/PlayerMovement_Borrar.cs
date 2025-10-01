using UnityEngine;
// Movimiento provisional para probar la cámara
public class PlayerMovement_Borrar : MonoBehaviour
{
    public float speed = 5f;       

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical");   
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

    }
}
