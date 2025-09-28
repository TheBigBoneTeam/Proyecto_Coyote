using Services;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    IHitStop hitstop;
    [SerializeField] float durationFreeze;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitstop = ServiceLocator.Instance.Get<IHitStop>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        hitstop.GeneralScreenFreeze(durationFreeze);
    }
}
