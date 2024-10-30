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
    public struct SpacesHostViewDeviceState : IInputStateTypeInfo
    {
        public FourCC format => new('M', 'Y', 'D', 'V');

        [FieldOffset(0)]
        [InputControl(displayName = "Mobile Display Position", name = "MobileDisplayPosition", layout = "vector3")]
        public Vector3 MobileDisplayPosition;

        [FieldOffset(12)]
        [InputControl(displayName = "Mobile Display Rotation", name = "MobileDisplayRotation", layout = "Quaternion")]
        public Quaternion MobileDisplayRotation;
    }


    [InputControlLayout(stateType = typeof(SpacesHostViewDeviceState))]
#if UNITY_EDITOR
    // Unityエディタで初期化処理を呼び出すのに必要
    [InitializeOnLoad]
#endif
    public class SpacesHostViewDevice : InputDevice
    {
        public Vector3Control MobileDisplayPosition { get; private set; }
        public QuaternionControl MobileDisplayRotation { get; private set; }

        static SpacesHostViewDevice()
        {
            InputSystem.RegisterLayout<SpacesHostViewDevice>();
            foreach (var inputDevice in InputSystem.devices)
                if (inputDevice is SpacesHostViewDevice)
                    return;
            var mobileDisplayDevice = InputSystem.AddDevice<SpacesHostViewDevice>();
            InputSystem.EnableDevice(mobileDisplayDevice);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            MobileDisplayPosition = GetChildControl<Vector3Control>("MobileDisplayPosition");
            MobileDisplayRotation = GetChildControl<QuaternionControl>("MobileDisplayRotation");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeInPlayer()
        {
        }
    }
}