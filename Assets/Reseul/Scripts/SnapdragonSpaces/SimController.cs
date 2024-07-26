using System;
using System.Collections;
using System.Collections.Generic;
using Reseul.Snapdragon.Spaces.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimController : MonoBehaviour
{

    public InputActionReference LeftStick_y;
    public InputActionReference LeftStick_x;
    public InputActionReference RightStick_y;
    public InputActionReference RightStick_x;
    public InputActionReference Touchpad_y;
    public InputActionReference Touchpad_x;
    public InputActionReference Button1;

    CanvasControllerInputDeviceState companionState;

    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_EDITOR
        companionState = new CanvasControllerInputDeviceState();
        companionState.trackingState = 1;
#endif
    }

    // Update is called once per frame
    void FixedUpdate()
    {
#if UNITY_EDITOR
        var leftStickValue_y = LeftStick_y.action.ReadValue<float>();
        var leftStickValue_x = LeftStick_x.action.ReadValue<float>();
        var rightStickValue_y = RightStick_y.action.ReadValue<float>();
        var rightStickValue_x = RightStick_x.action.ReadValue<float>();
        var touchpad_y = Touchpad_y.action.ReadValue<float>();
        var touchpad_x = Touchpad_x.action.ReadValue<float>();
        var button1Value = Button1.action.ReadValue<float>();

        var leftStickPosition = Vector2.zero;
        if (LeftStick_y.action.IsPressed())
        {
            leftStickPosition.y = leftStickValue_y;
        }

        if (LeftStick_x.action.IsPressed())
        {
            leftStickPosition.x = leftStickValue_x;
        }
        companionState.leftStickPosition = leftStickPosition;
            

        var RightStickPosition = Vector2.zero;
        if (RightStick_y.action.IsPressed())
        {
            RightStickPosition.y = rightStickValue_y;
        }

        if (RightStick_x.action.IsPressed())
        {
            RightStickPosition.x = rightStickValue_x;
        }
        
        companionState.rightStickPosition = RightStickPosition;

        var TouchpadPosition = Vector2.zero;
        if (Touchpad_y.action.IsPressed())
        {
            TouchpadPosition.y = touchpad_y;
        }

        if (Touchpad_x.action.IsPressed())
        {
            TouchpadPosition.x = touchpad_x;
        }
        companionState.touchpadPosition = TouchpadPosition;

        companionState.buttons = 0;
        if (Button1.action.IsPressed())
        {
            companionState.buttons = 1;
        }

        InputSystem.QueueStateEvent(InputSystem.GetDevice<CanvasControllerInputDevice>(), companionState);
#endif
    }
}
