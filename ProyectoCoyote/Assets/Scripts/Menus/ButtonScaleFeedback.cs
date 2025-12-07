using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScaleFeedback : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    ISelectHandler, IDeselectHandler
{
    [Header("Scale Settings")]
    public Vector3 normalScale = Vector3.one;
    public Vector3 highlightedScale = new Vector3(1.1f, 1.1f, 1.1f);
    public float scaleSpeed = 0.15f;

    bool isHighlighted = false;

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            isHighlighted ? highlightedScale : normalScale,
            Time.deltaTime * (1f / scaleSpeed)
        );
    }

    // MOUSE HOVER
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHighlighted = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHighlighted = false;
    }

    // GAMEPAD / KEYBOARD SELECT
    public void OnSelect(BaseEventData eventData)
    {
        isHighlighted = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isHighlighted = false;
    }
}
