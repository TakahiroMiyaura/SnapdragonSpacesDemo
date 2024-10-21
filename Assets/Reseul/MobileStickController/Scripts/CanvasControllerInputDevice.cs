// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace Reseul.Snapdragon.Spaces.Controllers
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [InputControlLayout(stateType = typeof(CanvasControllerInputDeviceState))]
    public class CanvasControllerInputDevice : InputDevice
    {
        public ButtonControl button1Press { get; private set; }

        public ButtonControl touchScreenPress { get; private set; }

        public ButtonControl leftStickPress { get; private set; }

        public ButtonControl rightStickPress { get; private set; }

        public Vector2Control touchScreenPosition { get; private set; }
        public Vector2Control touchScreenDelta { get; private set; }
        public Vector3Control touchScreenPosition3D { get; private set; }

        public Vector2Control touchRadius { get; private set; }
        public Vector2Control touchRadiusDelta { get; private set; }

        public Vector2Control leftStickPosition { get; private set; }

        public Vector2Control leftStickDelta { get; private set; }

        public Vector2Control rightStickPosition { get; private set; }
        public Vector2Control rightStickDelta { get; private set; }

        public IntegerControl trackingState { get; private set; }


        public ButtonControl newButton1Press { get; private set; }
        public ButtonControl newtouchScreenPress { get; private set; }
        public ButtonControl newrightStickPress { get; private set; }
        public ButtonControl newleftStickPress { get; private set; }

        public Vector2Control newleftStick { get; private set; }
        public Vector2Control newrightStick { get; private set; }

        public TouchControl newtouchState { get; private set; }

        static CanvasControllerInputDevice()
        {
            InputSystem.RegisterLayout<CanvasControllerInputDevice>();
            foreach (var inputDevice in InputSystem.devices)
                if (inputDevice is CanvasControllerInputDevice)
                    return;
            var canvasControllerInputDevice = InputSystem.AddDevice<CanvasControllerInputDevice>();
            InputSystem.EnableDevice(canvasControllerInputDevice);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            button1Press = GetChildControl<ButtonControl>("button1Press");
            touchScreenPress = GetChildControl<ButtonControl>("touchScreenPress");
            leftStickPress = GetChildControl<ButtonControl>("leftStickPress");
            rightStickPress = GetChildControl<ButtonControl>("rightStickPress");
            touchScreenPosition = GetChildControl<Vector2Control>("touchScreenPosition");
            touchScreenDelta = GetChildControl<Vector2Control>("touchScreenDelta");
            touchScreenPosition3D = GetChildControl<Vector3Control>("touchScreenPosition3D");
            touchRadius = GetChildControl<Vector2Control>("touchRadius");
            touchRadiusDelta = GetChildControl<Vector2Control>("touchRadiusDelta");
            leftStickPosition = GetChildControl<Vector2Control>("leftStickPosition");
            leftStickDelta = GetChildControl<Vector2Control>("leftStickDelta");
            rightStickPosition = GetChildControl<Vector2Control>("rightStickPosition");
            rightStickDelta = GetChildControl<Vector2Control>("rightStickDelta");
            trackingState = GetChildControl<IntegerControl>("trackingState");
            newButton1Press = GetChildControl<ButtonControl>("newButton1Press");
            newrightStickPress = GetChildControl<ButtonControl>("newrightStickPress");
            newleftStickPress = GetChildControl<ButtonControl>("newleftStickPress");
            newleftStick = GetChildControl<Vector2Control>("newleftStick");
            newrightStick = GetChildControl<Vector2Control>("newrightStick");
            newtouchState = GetChildControl<TouchControl>("newtouchState");
            newtouchScreenPress = GetChildControl<ButtonControl>("newtouchScreenPress");

        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeInPlayer()
        {
        }
    }
}