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
using UnityEngine.InputSystem.Utilities;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    [StructLayout(LayoutKind.Explicit)]
    public struct  MobileStickInputDeviceState : IInputStateTypeInfo
    {
        public FourCC format => new('M', 'Y', 'D', 'V');

        [FieldOffset(0)]
        [InputControl(displayName = "Button1 Press", name = "Button1Press", layout = "Button", bit = 0)]
        [InputControl(displayName = "Touch Screen Press", name = "TouchScreenPress", layout = "Button", bit = 1)]
        [InputControl(displayName = "Left Stick Press", name = "LeftStickPress", layout = "Button", bit = 2)]
        [InputControl(displayName = "Right Stick Press", name = "RightStickPress", layout = "Button", bit = 3)]
        public int Buttons;

        [FieldOffset(4)]
        [InputControl(displayName = "Left Stick", name = "LeftStick", layout = "stick")]
        public Vector2 LeftStick;
        
        [FieldOffset(12)]
        [InputControl(displayName = "Left Stick Delta", name = "LeftStickDelta", layout = "delta")]
        public Vector2 LeftStickDelta;
        
        [FieldOffset(24)]
        [InputControl(displayName = "Right Stick", name = "RightStick", layout = "stick")]
        public Vector2 RightStick;
        
        [FieldOffset(32)]
        [InputControl(displayName = "Right Stick Delta", name = "RightStickDelta", layout = "delta")]
        public Vector2 RightStickDelta;

        [FieldOffset(44)]
        [InputControl(displayName = "Touch Screen", name = "TouchScreenPosition", layout = "vector2")]
        public Vector2 TouchScreenPosition;

        [FieldOffset(52)]
        [InputControl(displayName = "Touch Screen(3D)", name = "TouchScreen3D", layout = "vector3")]
        public Vector3 TouchScreen3D;

        [FieldOffset(64)]
        [InputControl(displayName = "Touch Screen(3D - On Canvas)", name = "TouchScreen3DOnCanvas", layout = "vector3")]
        public Vector3 TouchScreen3DOnCanvas;

        [FieldOffset(76)]
        [InputControl(displayName = "Touch Screen Delta", name = "TouchScreenDelta", layout = "delta")]
        public Vector2 TouchScreenDelta;

        [FieldOffset(88)]
        [InputControl(displayName = "Touch State", name = "TouchState", layout = "Touch")]
        public TouchState TouchState;

    }


    [InputControlLayout(stateType = typeof(MobileStickInputDeviceState))]
#if UNITY_EDITOR
    // Unityエディタで初期化処理を呼び出すのに必要
    [InitializeOnLoad]
#endif
    public class MobileStickInputDevice : InputDevice 
    {
        public ButtonControl Button1Press { get; private set; }
        public ButtonControl TouchScreenPress { get; private set; }
        public ButtonControl LeftStickPress { get; private set; }
        public ButtonControl RightStickPress { get; private set; }

        public StickControl LeftStick { get; private set; }
        public DeltaControl LeftStickDelta { get; private set; }
        public StickControl RightStick { get; private set; }
        public DeltaControl RightStickDelta { get; private set; }
        public Vector2Control TouchScreenPosition { get; private set; }
        public Vector3Control TouchScreen3D { get; private set; }
        public Vector3Control TouchScreen3DOnCanvas { get; private set; }
        public DeltaControl TouchScreenDelta { get; private set; }
        public TouchControl TouchState { get; private set; }

        
        static MobileStickInputDevice()
        {
            InputSystem.RegisterLayout<MobileStickInputDevice>();
            foreach (var inputDevice in InputSystem.devices)
                if (inputDevice is MobileStickInputDevice)
                    return;
            var mobileStickInputDevice = InputSystem.AddDevice<MobileStickInputDevice>();
            InputSystem.EnableDevice(mobileStickInputDevice);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            Button1Press = GetChildControl<ButtonControl>("Button1Press");
            TouchScreenPress = GetChildControl<ButtonControl>("TouchScreenPress");
            LeftStickPress = GetChildControl<ButtonControl>("LeftStickPress");
            RightStickPress = GetChildControl<ButtonControl>("RightStickPress");

            LeftStick = GetChildControl<StickControl>("LeftStick");
            LeftStickDelta = GetChildControl<DeltaControl>("LeftStickDelta");
            RightStick = GetChildControl<StickControl>("RightStick");
            RightStickDelta = GetChildControl<DeltaControl>("RightStickDelta");
            TouchScreenPosition = GetChildControl<Vector2Control>("TouchScreenPosition");
            TouchScreen3D = GetChildControl<Vector3Control>("TouchScreen3D");
            TouchScreen3DOnCanvas = GetChildControl<Vector3Control>("TouchScreen3DOnCanvas");
            TouchScreenDelta = GetChildControl<DeltaControl>("TouchScreenDelta");
            TouchState = GetChildControl<TouchControl>("TouchState");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeInPlayer()
        {
        }
    }
}