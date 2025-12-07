using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectDefaultButton : MonoBehaviour
{
 

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject.GetComponent<Selectable>().gameObject);
    }
}