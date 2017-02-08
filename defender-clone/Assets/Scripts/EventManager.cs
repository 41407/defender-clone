using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void InputAction(Vector2 position, int touchId);
    public static event InputAction OnButtonDown;
    public static event InputAction OnButtonUp;
    public static event InputAction OnButtonHold;
    private const int NUMBER_OF_MOUSE_BUTTONS = 5;
    private RuntimePlatform platform;

    void Awake()
    {
        platform = Application.platform;
    }

    void Update()
    {
        if (platform != RuntimePlatform.IPhonePlayer)
        {
            SendMouseEvents();
        }
        else
        {
            SendTouchEvents();
        }
    }

    private void SendMouseEvents()
    {
        for (int button = 0; button < NUMBER_OF_MOUSE_BUTTONS; button++)
        {
            if (Input.GetMouseButtonDown(button))
            {
                if (OnButtonDown != null)
                {
                    OnButtonDown(Input.mousePosition, button);
                }
            }
            if (Input.GetMouseButton(button))
            {
                if (OnButtonHold != null)
                {
                    OnButtonHold(Input.mousePosition, button);
                }
            }
            if (Input.GetMouseButtonUp(button))
            {
                if (OnButtonUp != null)
                {
                    OnButtonUp(Input.mousePosition, button);
                }
            }
        }
    }

    private void SendTouchEvents()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (OnButtonDown != null)
                {
                    OnButtonDown(touch.position, touch.fingerId);
                }
            }
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (OnButtonHold != null)
                {
                    OnButtonHold(touch.position, touch.fingerId);
                }
            }
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (OnButtonUp != null)
                {
                    OnButtonUp(touch.position, touch.fingerId);
                }
            }
        }
    }
}
