// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Reseul.Snapdragon.Spaces.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class MobileStickControllerSimulator : MonoBehaviour
    {
        private Vector2 touchScreenPos = new(1170, 540);
        private MobileStickInputDeviceState state;

#if UNITY_EDITOR
        // Update is called once per frame
        private void FixedUpdate()
        {
            var leftStickPosition = Vector2.zero;
            state.Buttons &= ~(1 << 2);
            if (Input.GetKey(KeyCode.W))
            {
                leftStickPosition.y = 1;
                state.Buttons |= 1 << 2;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                leftStickPosition.y = -1;
                state.Buttons |= 1 << 2;
            }

            if (Input.GetKey(KeyCode.A))
            {
                leftStickPosition.x = -1;
                state.Buttons |= 1 << 2;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                leftStickPosition.x = 1;
                state.Buttons |= 1 << 2;
            }

            state.LeftStick = leftStickPosition;

            var rightStickPosition = Vector2.zero;
            state.Buttons &= ~(1 << 3);
            if (Input.GetKey(KeyCode.U))
            {
                rightStickPosition.y = 1;
                state.Buttons |= 1 << 3;
            }
            else if (Input.GetKey(KeyCode.J))
            {
                rightStickPosition.y = -1;
                state.Buttons |= 1 << 3;
            }

            if (Input.GetKey(KeyCode.H))
            {
                rightStickPosition.x = -1;
                state.Buttons |= 1 << 3;
            }
            else if (Input.GetKey(KeyCode.K))
            {
                rightStickPosition.x = 1;
                state.Buttons |= 1 << 3;
            }

            state.RightStick = rightStickPosition;

            if (Input.GetKey(KeyCode.Y))
                state.Buttons |= 1 << 0;
            else
                state.Buttons &= ~(1 << 0);

            var touchScreen = Vector2.zero;
            var isPressed = false;
            state.Buttons &= ~(1 << 1);
            if (Input.GetKey(KeyCode.UpArrow))
            {
                touchScreen.y = 1;
                state.Buttons |= 1 << 1;
                isPressed = true;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                touchScreen.y = -1;
                state.Buttons |= 1 << 1;
                isPressed = true;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                touchScreen.x = -1;
                state.Buttons |= 1 << 1;
                isPressed = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                touchScreen.x = 1;
                state.Buttons |= 1 << 1;
                isPressed = true;
            }

            touchScreenPos += touchScreen * 10;
            state.TouchScreenPosition = touchScreenPos;
            if (isPressed)
                InputSystem.QueueDeltaStateEvent(InputSystem.GetDevice<MobileStickInputDevice>().TouchScreenDelta,
                    touchScreen * 10,
                    Time.realtimeSinceStartup);

            InputSystem.QueueStateEvent(InputSystem.GetDevice<MobileStickInputDevice>(), state);
        }
#endif

    }
}