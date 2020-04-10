using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoystick : Joystick
{
    Vector2 joystickPosition = Vector2.zero;
    private Camera cam = new Camera();

    public Transform obj;
    [Range(0, 5)]
    public float speed = 1;
    public dir[] dirs = new dir[2]; 

    void Start()
    {
        joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickPosition;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;

        
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    private void Update()
    {
        obj.Rotate(inputVector.y * speed * (int)dirs[0], inputVector.x * speed * (int)dirs[1], 0, new Space());

        //obj.RotateAroundLocal(new Vector3(0, 1, 0), inputVector.x * speed * 0.1f);
        //obj.RotateAroundLocal(new Vector3(1, 0, 0), inputVector.y * speed * 0.1f);

    }
}

public enum dir
{
    NORMAL = 1,
    REVERSE = -1
}