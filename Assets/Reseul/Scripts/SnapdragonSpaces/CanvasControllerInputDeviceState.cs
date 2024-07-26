// Copyright (c) 2023 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public struct CanvasControllerInputDeviceState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('M', 'Y', 'D', 'V');

        [InputControl(displayName = "button1", name = "button1", layout ="Button", bit = 0)]
        [InputControl(displayName = "Touchpad Click", name = "touchpadClick", layout = "Button", bit = 1)]
        [InputControl(displayName = "Left Stick Click", name = "leftStickClick", layout = "Button", bit = 2)]
        [InputControl(displayName = "Right Stick Click", name = "rightStickClick", layout = "Button", bit = 3)]
        public int buttons;

        [InputControl(displayName = "Touchpad", name = "touchpadPosition", layout = "Vector2")]
        public Vector2 touchpadPosition;

        [InputControl(displayName = "Left Stick", name = "leftStickPosition", layout = "Vector2")]
        public Vector2 leftStickPosition;

        [InputControl(displayName = "Right Stick", name = "rightStickPosition", layout = "Vector2")]
        public Vector2 rightStickPosition;

        [InputControl(displayName = "Device Position", name ="devicePosition")]
        public Vector3 devicePosition;

        [InputControl(displayName = "Device Rotation", name ="deviceRotation")]
        public Quaternion deviceRotation;

        [InputControl(displayName = "Tracking State", name = "trackingState", layout ="Integer")]
        public int trackingState;
    }
}