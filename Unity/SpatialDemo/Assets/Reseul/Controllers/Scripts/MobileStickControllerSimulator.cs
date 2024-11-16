// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class MobileStickControllerSimulator : MonoBehaviour
    {
        private OnScreenTouch3D onScreenTouch3D;
        private MobileStickInputDeviceState state;
        private Vector2 touchScreenPos = Vector2.zero;

        private void Awake()
        {
            touchScreenPos = new Vector2(Screen.currentResolution.width / 2f, Screen.currentResolution.height / 2f);
            onScreenTouch3D = FindObjectOfType<OnScreenTouch3D>();
        }

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
                touchScreen.y = 10;
                state.Buttons |= 1 << 1;
                isPressed = true;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                touchScreen.y = -10;
                state.Buttons |= 1 << 1;
                isPressed = true;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                touchScreen.x = -10;
                state.Buttons |= 1 << 1;
                isPressed = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                touchScreen.x = 10;
                state.Buttons |= 1 << 1;
                isPressed = true;
            }

            touchScreenPos += touchScreen;
            state.TouchScreenPosition = touchScreenPos;
            if (isPressed)
            {
                InputSystem.QueueDeltaStateEvent(InputSystem.GetDevice<MobileStickInputDevice>().TouchScreenDelta,
                    touchScreen,
                    Time.realtimeSinceStartup);
                state.TouchScreen3D = onScreenTouch3D.Calculate3DPositionFrom2D(touchScreenPos);
            }

            InputSystem.QueueStateEvent(InputSystem.GetDevice<MobileStickInputDevice>(), state);
        }
#endif
    }
}