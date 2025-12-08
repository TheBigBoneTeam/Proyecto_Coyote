using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScaleFeedback : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    ISelectHandler, IDeselectHandler
{
    [Header("Scale Settings")]
    [SerializeField] Transform target;
    Vector3 normalScale;
    Vector3 highlightedScale;
    [SerializeField] float scaleFactor = 1.2f;
    public float scaleSpeed = 0.15f;

    bool isHighlighted = false;

    void Start()
    {
        if (target == null)
        {
            target = transform;
        }
        normalScale = target.localScale;
        highlightedScale = target.localScale * scaleFactor;
    }
    void Update()
    {
        target.localScale = Vector3.Lerp(
            target.localScale,
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
