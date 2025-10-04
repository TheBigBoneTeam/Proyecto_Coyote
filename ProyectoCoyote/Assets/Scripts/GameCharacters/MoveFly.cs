using UnityEngine;

public class MoveFly : MonoBehaviour
{
    Vector3 intiialpos;
 [SerializeField]   float speed;
    private void Start()
    {
        intiialpos = transform.position;
    }
    private void Update()
    {
        transform.position += new Vector3(speed,0,0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Respawn")
        {
            transform.position = intiialpos;
        }
    }
}