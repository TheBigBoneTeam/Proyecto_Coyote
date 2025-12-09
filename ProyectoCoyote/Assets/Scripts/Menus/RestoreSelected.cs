using UnityEngine;
using UnityEngine.EventSystems;

public class RestoreSelection : MonoBehaviour
{
    public GameObject defaultSelected;
    private GameObject lastSelected;

    void Start()
    {
        lastSelected = defaultSelected;
        EventSystem.current.SetSelectedGameObject(defaultSelected);
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
        else
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
    }
}

