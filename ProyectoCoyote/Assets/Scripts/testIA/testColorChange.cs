using BehaviourAPI.Core;
using UnityEngine;

public class testColorChange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    [SerializeField] bool val;
    public Status Blue()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        return Status.Success;

    }
    public Status Red()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        return Status.Success;

    }
    public Status Green()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
        return Status.Success;

    }
    public Status White()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        return Status.Success;

    }
    public Status Shit()
    {
        return Status.Running;

    }
    public bool mouseClick()
    {
        print("checkMouse" + Input.GetMouseButtonDown(0));
        return Input.GetMouseButtonDown(0);
    }
    public bool isTrue() => val;
    public bool isNotTrue() => !val;

}
