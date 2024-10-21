using System;
using System.Collections;
using System.Collections.Generic;
using Reseul.Snapdragon.Spaces.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimController : MonoBehaviour
{

    [SerializeField]
    private Camera _arCamera;

    [SerializeField]
    private RectTransform _camCanvas;

    public InputActionReference LeftStick_y;
    public InputActionReference LeftStick_x;
    public InputActionReference RightStick_y;
    public InputActionReference RightStick_x;
    public InputActionReference Touchpad_y;
    public InputActionReference Touchpad_x;
    public InputActionReference Button1;
    public InputActionReference LeftStick;
    public InputActionReference RightStick;

    CanvasControllerInputDeviceState companionState;

    private Vector2 _touchpadPos = new Vector2(1170, 540);

    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_EDITOR
        companionState = new CanvasControllerInputDeviceState();
        companionState.trackingState = 1;
#endif
    }

    void Start()
    {
        //Button1.action.started += ctx =>
        //{
        //   CanvasController.Instance.SendButton1PressEvent(1);
        //};
        //Button1.action.canceled += ctx =>
        //{
        //    CanvasController.Instance.SendButton1PressEvent(0);
        //};
        //LeftStick.action.started += ctx =>
        //{
        //    CanvasController.Instance.SendLeftStickPositionEvent(1,ctx.ReadValue<Vector2>());
        //};
        //LeftStick.action.performed += ctx =>
        //{
        //    CanvasController.Instance.SendLeftStickPositionEvent(1, ctx.ReadValue<Vector2>());
        //};
        //LeftStick.action.canceled += ctx =>
        //{
        //    CanvasController.Instance.SendLeftStickPositionEvent(0, ctx.ReadValue<Vector2>());
        //};
        //RightStick.action.started += ctx =>
        //{
        //    CanvasController.Instance.SendRightStickPositionEvent(1, ctx.ReadValue<Vector2>());
        //};
        //RightStick.action.performed += ctx =>
        //{
        //    CanvasController.Instance.SendRightStickPositionEvent(1, ctx.ReadValue<Vector2>());
        //};
        //RightStick.action.canceled += ctx =>
        //{
        //    CanvasController.Instance.SendRightStickPositionEvent(0, ctx.ReadValue<Vector2>());
        //};
    }

    // Update is called once per frame
    void FixedUpdate()
    {
#if UNITY_EDITOR
        bool isChanged = false;
        //var leftStickPosition = Vector2.zero;
        //if (LeftStick_y.action.IsPressed() || LeftStick_x.action.IsPressed())
        //{
        //    isChanged = true;
        //    leftStickPosition.y = LeftStick_y.action.ReadValue<float>();
        //    companionState.NewButtons |= (ushort)1<<2;
            

        //    leftStickPosition.x = LeftStick_x.action.ReadValue<float>();
        //    companionState.NewButtons |= (ushort)1 << 2;

        //    companionState.newleftStick = leftStickPosition;
        //}

        if (LeftStick.action.IsPressed())
        {
            companionState.NewButtons |= (ushort)1 << 2;
            companionState.newleftStick = LeftStick.action.ReadValue<Vector2>();
        }
        else
        {
            companionState.NewButtons &= (ushort)1 << 2;
        }


        //var rightStickPosition = Vector2.zero;
        //if (RightStick_y.action.IsPressed() || RightStick_x.action.IsPressed())
        //{ ;
        //    isChanged = true;
        //    rightStickPosition.y = RightStick_y.action.ReadValue<float>();
        //    companionState.NewButtons |= (ushort)1 << 3;

        //    rightStickPosition.x = RightStick_x.action.ReadValue<float>();
        //    companionState.NewButtons |= (ushort)1 << 3;

        //    companionState.newrightStick = rightStickPosition;
        //}

        if (RightStick.action.IsPressed())
        {
            companionState.NewButtons |= (ushort)1 << 3;
            companionState.newrightStick = RightStick.action.ReadValue<Vector2>();
        }
        else
        {
            companionState.NewButtons &= (ushort)1 << 3;
        }

        var touchpadPosition = Vector2.zero;
        if (Touchpad_y.action.IsPressed() || Touchpad_x.action.IsPressed())
        {
            touchpadPosition.y = Touchpad_y.action.ReadValue<float>();
            companionState.NewButtons |= (ushort)1 << 1;

            touchpadPosition.x = Touchpad_x.action.ReadValue<float>();
            companionState.NewButtons |= (ushort)1 << 1;

            _touchpadPos += touchpadPosition;
            companionState.touchScreenPosition = _touchpadPos;
        }

        //if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_camCanvas, _touchpadPos, _arCamera,
        //        out Vector3 result))
        //{
        //    companionState.touchScreenPosition3D = result;
        //}
        //companionState.touchScreenPosition3D = Vector3.zero;

        if (Button1.action.IsPressed())
        {
            companionState.NewButtons |= (ushort)1 << 0;
        }
        else
        {
            companionState.NewButtons &= (ushort)1 << 0;
        }
        
            InputSystem.QueueStateEvent(InputSystem.GetDevice<CanvasControllerInputDevice>(), companionState);
#endif
    }
}
