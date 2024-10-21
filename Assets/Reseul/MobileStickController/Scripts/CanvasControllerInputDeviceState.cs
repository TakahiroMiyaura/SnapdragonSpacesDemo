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
        
        [InputControl(displayName = "button1Press", name = "button1Press", layout ="Button", bit = 0)]
        [InputControl(displayName = "Touch Screen Press", name = "touchScreenPress", layout = "Button", bit = 1)]
        [InputControl(displayName = "Left Stick Press", name = "leftStickPress", layout = "Button", bit = 2)]
        [InputControl(displayName = "Right Stick Press", name = "rightStickPress", layout = "Button", bit = 3)]
        public int buttons;

        [InputControl(displayName = "Touch Screen Position", name = "touchScreenPosition", layout = "Vector2")]
        public Vector2 touchScreenPosition;

        [InputControl(displayName = "Touch Screen Delta", name = "touchScreenDelta", layout = "Delta")]
        public Vector2 touchScreenDelta;

        [InputControl(displayName = "Touch Screen Position(3D)", name = "touchScreenPosition3D")]
        public Vector3 touchScreenPosition3D;

        [InputControl(displayName = "Touch Radius", name = "touchRadius", layout = "Vector2")]
        public Vector2 touchRadius;

        [InputControl(displayName = "Touch Radius Delta", name = "touchRadiusDelta", layout = "Delta")]
        public Vector2 touchRadiusDelta;

        [InputControl(displayName = "Left Stick", name = "leftStickPosition", layout = "Vector2")]
        public Vector2 leftStickPosition;
        
        [InputControl(displayName = "Left Stick Delta", name = "leftStickDelta", layout = "Delta")]
        public Vector2 leftStickDelta;

        [InputControl(displayName = "Right Stick", name = "rightStickPosition", layout = "Vector2")]
        public Vector2 rightStickPosition;

        [InputControl(displayName = "Right Stick Delta", name = "rightStickDelta", layout = "Delta")]
        public Vector2 rightStickDelta;

        [InputControl(displayName = "Tracking State", name = "trackingState", layout ="Integer")]
        public int trackingState;

        [InputControl(displayName = "New Right Stick", name = "newrightStick", layout = "stick")]
        public Vector2 newrightStick;

        [InputControl(displayName = "New Left Stick", name = "newleftStick", layout = "stick")]
        public Vector2 newleftStick;

        [InputControl(displayName = "New Button1 Press", name = "newButton1Press", layout = "Button", bit = 0)]
        [InputControl(displayName = "New Touch Screen Press", name = "newtouchScreenPress", layout = "Button", bit = 1)]
        [InputControl(displayName = "New Left Stick Press", name = "newleftStickPress", layout = "Button", bit = 2)]
        [InputControl(displayName = "New Right Stick Press", name = "newrightStickPress", layout = "Button", bit = 3)]
        public int NewButtons;

        [InputControl(displayName = "New Touch State", name = "newtouchState", layout = "touch")]
        public TouchState NewTouchState;

    }
}