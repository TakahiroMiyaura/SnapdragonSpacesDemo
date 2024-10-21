// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.Utilities;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    [StructLayout(LayoutKind.Explicit)]
    public struct  MyCustomDeviceState : IInputStateTypeInfo
    {
        [FieldOffset(6)]
        [InputControl(displayName = "Button1 Press", name = "Button1Press", layout = "Button", bit = 0)]
        [InputControl(displayName = "Touch Screen Press", name = "TouchScreenPress", layout = "Button", bit = 1)]
        [InputControl(displayName = "Left Stick Press", name = "LeftStickPress", layout = "Button", bit = 2)]
        [InputControl(displayName = "Right Stick Press", name = "RightStickPress", layout = "Button", bit = 3)]
        public int Buttons;

        [FieldOffset(5)]
        [InputControl(displayName = "Left Stick", name = "LeftStick", layout = "stick")]
        public Vector2 LeftStick;

        [FieldOffset(4)]
        [InputControl(displayName = "Right Stick", name = "RightStick", layout = "stick")]
        public Vector2 RightStick;

        [FieldOffset(0)]
        [InputControl(displayName = "Touch Screen", name = "TouchScreenPosition", layout = "vector2")]
        public Vector2 TouchScreenPosition;

        [FieldOffset(2)]
        [InputControl(displayName = "Touch Screen(3D)", name = "TouchScreen3D", layout = "vector3")]
        public Vector2 TouchScreen3D;

        [FieldOffset(1)]
        [InputControl(displayName = "Touch Screen Delta", name = "TouchScreenDelta", layout = "delta")]
        public Vector2 TouchScreenDelta;

        [FieldOffset(3)]
        [InputControl(displayName = "Touch State", name = "TouchState", layout = "Touch")]
        public TouchState TouchState;

        public FourCC format => new('M', 'Y', 'D', 's');
    }


    [InputControlLayout(stateType = typeof(MyCustomDeviceState))]
#if UNITY_EDITOR
    // Unityエディタで初期化処理を呼び出すのに必要
    [InitializeOnLoad]
#endif
    public class MyCustomDevice : InputDevice 
    {
        public ButtonControl Button1Press { get; private set; }
        public ButtonControl TouchScreenPress { get; private set; }
        public ButtonControl LeftStickPress { get; private set; }
        public ButtonControl RightStickPress { get; private set; }

        public StickControl LeftStick { get; private set; }
        public StickControl RightStick { get; private set; }
        public Vector2Control TouchScreenPosition { get; private set; }
        public Vector3Control TouchScreen3D { get; private set; }
        public DeltaControl TouchScreenDelta { get; private set; }
        public TouchControl TouchState { get; private set; }

        
        static MyCustomDevice()
        {
            InputSystem.RegisterLayout<MyCustomDevice>();
            foreach (var inputDevice in InputSystem.devices)
                if (inputDevice is MyCustomDevice)
                    return;
            var canvasControllerInputDevice = InputSystem.AddDevice<MyCustomDevice>();
            InputSystem.EnableDevice(canvasControllerInputDevice);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            Button1Press = GetChildControl<ButtonControl>("Button1Press");
            TouchScreenPress = GetChildControl<ButtonControl>("TouchScreenPress");
            LeftStickPress = GetChildControl<ButtonControl>("LeftStickPress");
            RightStickPress = GetChildControl<ButtonControl>("RightStickPress");

            LeftStick = GetChildControl<StickControl>("LeftStick");
            RightStick = GetChildControl<StickControl>("RightStick");
            TouchScreenPosition = GetChildControl<Vector2Control>("TouchScreenPosition");
            TouchScreen3D = GetChildControl<Vector3Control>("TouchScreen3D");
            TouchScreenDelta = GetChildControl<DeltaControl>("TouchScreenDelta");
            TouchState = GetChildControl<TouchControl>("TouchState");
        }

        public void OnUpdate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            
            Debug.Log($"Left:{LeftStickPress.isPressed},Right:{RightStickPress.isPressed},Button1:{Button1Press.isPressed}");
            var state = new MyCustomDeviceState();
            var isStickOperation = false;
            var buttons = 0;
            if (Button1Press.isPressed)
            {
                buttons |= 1 << 0;
                buttons &= ~(1 << 1);
                isStickOperation = true;
            }
            else if (!Button1Press.isPressed)
            {
                buttons &= ~(1 << 0);
            }

            if (LeftStickPress.isPressed)
            {
                buttons |= 1 << 2;
                buttons &= ~(1 << 1);
                isStickOperation = true;
            }
            else if (!LeftStickPress.isPressed)
            {
                buttons &= ~(1 << 2);
            }
            if (RightStickPress.isPressed)
            {
                buttons |= 1 << 3;
                buttons &= ~(1 << 1);
                isStickOperation = true;
            }
            else if (!RightStickPress.isPressed)
            {
                buttons &= ~(1 << 3);
            }

            if (!isStickOperation)
            {
                var touch = Touchscreen.current;
                if (touch != null)
                {
                    if (touch.press.isPressed)
                    {
                        buttons |= 1 << 1;
                    }
                    else if (!touch.press.isPressed)
                    {
                        buttons &= ~(1 << 1);
                    }
                    
                    state.Buttons = buttons;
                    state.LeftStick = LeftStickPress.isPressed ? LeftStick.ReadValue() : Vector2.zero;
                    state.RightStick = RightStickPress.isPressed? RightStick.ReadValue() : Vector2.zero;
                    state.TouchScreenPosition = touch.primaryTouch.position.ReadValue();
                    //state.TouchScreen3D = touch.position.ReadValue();
                    state.TouchScreenDelta = touch.primaryTouch.delta.ReadValue();
                    state.TouchState = touch.primaryTouch.value;
                    switch (state.TouchState.phase)
                    {
                        case TouchPhase.Began:
                            Debug.Log($"Press:{touch.press.isPressed},TouchState:{state.TouchState.phase},Delta:{touch.primaryTouch.delta.ReadValue()},Pos:{touch.primaryTouch.position.ReadValue()}");
                            break;
                        case TouchPhase.Ended:
                            Debug.Log($"Press:{touch.press.isPressed},TouchState:{state.TouchState.phase},Delta:{touch.primaryTouch.delta.ReadValue()},Pos:{touch.primaryTouch.position.ReadValue()}");
                            break;
                        case TouchPhase.Moved:
                            InputSystem.QueueDeltaStateEvent(this.TouchScreenDelta, touch.primaryTouch.delta.ReadValue(),
                                Time.realtimeSinceStartup);
                            Debug.Log($"Press:{touch.press.isPressed},TouchState:{state.TouchState.phase},Delta:{touch.primaryTouch.delta.ReadValue()},Pos:{touch.primaryTouch.position.ReadValue()}");
                            break;
                        case TouchPhase.Stationary:
                            InputSystem.QueueDeltaStateEvent(this.TouchScreenDelta, touch.primaryTouch.delta.ReadValue(),
                                Time.realtimeSinceStartup);
                            Debug.Log($"Press:{touch.press.isPressed},TouchState:{state.TouchState.phase},Delta:{touch.primaryTouch.delta.ReadValue()},Pos:{touch.primaryTouch.position.ReadValue()}");
                            break;
                        case TouchPhase.Canceled:
                            Debug.Log($"Press:{touch.press.isPressed},TouchState:{state.TouchState.phase},Delta:{touch.primaryTouch.delta.ReadValue()},Pos:{touch.primaryTouch.position.ReadValue()}");
                            break;
                        case TouchPhase.None:
                            Debug.Log($"Press:{touch.press.isPressed},TouchState:{state.TouchState.phase},Delta:{touch.primaryTouch.delta.ReadValue()},Pos:{touch.primaryTouch.position.ReadValue()}");
                            break;
                    }
                }
            }

            InputSystem.QueueStateEvent(this, state);

        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeInPlayer()
        {
        }
    }
}