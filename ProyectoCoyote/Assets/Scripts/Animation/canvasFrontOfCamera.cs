using UnityEngine;

public class canvasFrontOfCamera : MonoBehaviour
{
    [SerializeField] float dist;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,Camera.main.transform.rotation * Vector3.up);
        transform.GetChild(0).transform.position = transform.position + (Camera.main.transform.position - transform.position) * dist;
    }
}
