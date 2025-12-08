using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class HitStopComponent : AHitstopBase
{
    private Rigidbody _rigidbody;
    Vector3 _storedVelocity, _storedAngVelocity;
    Animator[] animators;
    float[] savedAnimSpeed;
    NavMeshAgent agent;
    float navMeshSpeed;
    public override void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody>();
        animators = GetComponentsInChildren<Animator>();
        savedAnimSpeed = new float[animators.Length];
        agent = GetComponent<NavMeshAgent>();
    }
    protected override void Stop()
    {
        if (_rigidbody != null)
        {
            _storedVelocity = _rigidbody.linearVelocity;
            _storedAngVelocity = _rigidbody.angularVelocity;
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
        }
        if (animators != null)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                savedAnimSpeed[i] = animators[i].speed;
                animators[i].speed = 0;
            }
        }
        if (agent != null)
        {
            navMeshSpeed = agent.speed;
            agent.speed = 0;
        }
        // gameObject.GetComponent<Renderer>().material.color = new Color(138f/255, 43f / 255, 226f / 255, 1);

        print("stop");

    }
    protected override void Continue()
    {
        if (_rigidbody != null)
        {
            //_rigidbody.isKinematic = false;
            _rigidbody.angularVelocity = _storedAngVelocity;
            _rigidbody.linearVelocity = _storedVelocity;
            // gameObject.GetComponent<Renderer>().material.color = Color.gray;
        }
        if (animators != null)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].speed = savedAnimSpeed[i];
            }
        }
        if (agent != null)
            agent.speed = navMeshSpeed;

    }
    private void Update()
    {

    }
}
