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

        static CanvasControllerInputDevice()
        {
            InputSystem.RegisterLayout<CanvasControllerInputDevice>();
            foreach (var inputDevice in InputSystem.devices)
                if (inputDevice is CanvasControllerInputDevice)
                    return;
            InputSystem.AddDevice<CanvasControllerInputDevice>();
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
        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeInPlayer()
        {
        }
    }
}