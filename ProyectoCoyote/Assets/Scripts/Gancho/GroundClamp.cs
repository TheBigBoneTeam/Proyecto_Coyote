using UnityEngine;

[RequireComponent(typeof(Transform))]
public class GroundClamp : MonoBehaviour
{
    [Tooltip("Capa del suelo")]
    public LayerMask groundMask;

    [Tooltip("Distancia máxima para buscar el suelo debajo")]
    public float maxCheckDistance = 5f;

    [Tooltip("Separación mínima para evitar penetración")]
    public float skin = 0.02f;

    void FixedUpdate()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, maxCheckDistance, groundMask))
        {
            float groundY = hit.point.y + skin;

            // Si el objeto está por debajo del suelo, lo subimos
            if (transform.position.y < groundY)
            {
                transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            }
        }
    }
}
