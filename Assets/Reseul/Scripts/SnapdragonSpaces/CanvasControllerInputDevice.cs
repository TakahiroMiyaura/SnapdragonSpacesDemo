// Copyright (c) 2023 Takahiro Miyaura
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
        public ButtonControl button1 { get; private set; }

        public ButtonControl touchpadClick { get; private set; }

        public ButtonControl leftStickClick { get; private set; }

        public ButtonControl rightStickClick { get; private set; }

        public Vector2Control touchpadPosition { get; private set; }


        public Vector2Control leftStickPosition { get; private set; }


        public Vector2Control rightStickPosition { get; private set; }

        public Vector3Control devicePosition { get; private set; }

        public QuaternionControl deviceRotation { get; private set; }

        public IntegerControl trackingState { get; private set; }

        static CanvasControllerInputDevice()
        {
            InputSystem.RegisterLayout<CanvasControllerInputDevice>();
            foreach(InputDevice inputDevice in InputSystem.devices)
            {
                if (inputDevice is CanvasControllerInputDevice)
                {
                    return;
                }
            }
            InputSystem.AddDevice<CanvasControllerInputDevice>();
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            button1 = GetChildControl<ButtonControl>("button1");
            touchpadClick = GetChildControl<ButtonControl>("touchpadClick");
            leftStickClick = GetChildControl<ButtonControl>("leftStickClick");
            rightStickClick = GetChildControl<ButtonControl>("rightStickClick");
            touchpadPosition = GetChildControl<Vector2Control>("touchpadPosition");
            leftStickPosition = GetChildControl<Vector2Control>("leftStickPosition");
            rightStickPosition = GetChildControl<Vector2Control>("rightStickPosition");
            devicePosition = GetChildControl<Vector3Control>("devicePosition");
            deviceRotation = GetChildControl<QuaternionControl>("deviceRotation");
            trackingState = GetChildControl<IntegerControl>("trackingState");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeInPlayer() { }
    }
}