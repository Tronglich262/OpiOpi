using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform background;   // Nền Joystick
    public RectTransform handle;       // Nút điều khiển
    public float handleRange = 100f;   // Bán kính di chuyển tối đa của handle

    private Vector2 inputVector;

    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;
    public Vector2 Direction => inputVector;

    private Vector2 startPos;

    void Start()
    {
        if (background == null)
            background = GetComponent<RectTransform>();

        if (handle != null)
            startPos = handle.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out pos);

        pos = Vector2.ClampMagnitude(pos, handleRange);
        inputVector = pos / handleRange;

        // di chuyển handle
        if (handle != null)
            handle.anchoredPosition = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        if (handle != null)
            handle.anchoredPosition = startPos; // reset về giữa
    }
}
