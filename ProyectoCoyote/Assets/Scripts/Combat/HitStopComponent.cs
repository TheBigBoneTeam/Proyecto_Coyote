using UnityEngine;
using UnityEngine.TextCore.Text;

public class HitStopComponent : AHitstopBase
{
    private Rigidbody _rigidbody;
    Vector3 _storedVelocity,_storedAngVelocity;

    public override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody>();
    }
    protected override void Stop()
    {
        _storedVelocity = _rigidbody.linearVelocity;
        _storedAngVelocity = _rigidbody.angularVelocity;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        gameObject.GetComponent<Renderer>().material.color = new Color(138f/255, 43f / 255, 226f / 255, 1);

        print("stop");

    }
    protected override void Continue()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.angularVelocity = _storedAngVelocity;
        _rigidbody.linearVelocity= _storedVelocity;
        gameObject.GetComponent<Renderer>().material.color = Color.gray;

    }
}
