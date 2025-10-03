using UnityEngine;

public class TestInput : MonoBehaviour
{
    DamageReceiver receiver;
    private void Start()
    {
        receiver = GetComponent<DamageReceiver>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            receiver.setDirection(HitDirections.Left);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            receiver.setDirection(HitDirections.Rigth);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            receiver.setDirection(HitDirections.Back);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            receiver.setDodge(true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            receiver.setDodge(false);
        }
    }
}
