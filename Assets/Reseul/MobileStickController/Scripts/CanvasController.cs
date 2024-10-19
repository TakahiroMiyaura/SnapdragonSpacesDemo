// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class CanvasController : MonoBehaviour
    {
        private static CanvasController _instanceObj;
        private CanvasControllerInputDevice _inputDevice;
        private CanvasControllerInputDeviceState _companionState;
        public UnityEvent OnLeftStickEnd;
        public UnityEvent OnRightStickEnd;
        public UnityEvent OnTouchScreenEnd;

        public string DebugText { get; private set; }

        public static CanvasController Instance
        {
            get
            {
                if (_instanceObj == null) _instanceObj = FindObjectOfType<CanvasController>();
                return _instanceObj;
            }
        }

        private void Awake()
        {
            _companionState = new CanvasControllerInputDeviceState
            {
                trackingState = 1
            };
        }

        private void OnEnable()
        {
            _inputDevice ??= InputSystem.GetDevice<CanvasControllerInputDevice>();
        }


        public void SendButton1PressEvent(int phase)
        {
            var bit = 1 << 0;
            if (phase != 0)
                _companionState.buttons |= (ushort)bit;
            else
                _companionState.buttons &= (ushort)~bit;
            DebugText = $"{_companionState.buttons:B4}";
            InputSystem.QueueStateEvent(_inputDevice, _companionState);
        }

        public void SendTouchScreenPositionEvent(int phase, Vector2 position)
        {
            var bit = 1 << 1;
            if (phase != 0)
            {
                _companionState.buttons |= (ushort)bit;
            }
            else
            {
                _companionState.buttons &= (ushort)~bit;
                OnTouchScreenEnd?.Invoke();
            }

            DebugText = $"{_companionState.buttons:B4}";

            _companionState.touchScreenPosition.x = position.x;
            _companionState.touchScreenPosition.y = position.y;

            InputSystem.QueueStateEvent(_inputDevice, _companionState);
        }

        public void SendTouchScreenDeltaEvent(int phase, Vector2 delta)
        {
            InputSystem.QueueDeltaStateEvent(_inputDevice.touchScreenDelta, delta, Time.realtimeSinceStartup);
        }

        public void SendTouchScreenPosition3DEvent(int phase, Vector3 normalizedPosition)
        {
            var bit = 1 << 1;
            if (phase != 0)
            {
                _companionState.buttons |= (ushort)bit;
            }
            else
            {
                _companionState.buttons &= (ushort)~bit;
                OnTouchScreenEnd?.Invoke();
            }

            DebugText = $"{_companionState.buttons:B4}";

            _companionState.touchScreenPosition3D = normalizedPosition;

            InputSystem.QueueStateEvent(_inputDevice, _companionState);
        }

        public void SendTouchRadiusEvent(int phase, Vector2 position)
        {
            var bit = 1 << 1;
            if (phase != 0)
            {
                _companionState.buttons |= (ushort)bit;
            }
            else
            {
                _companionState.buttons &= (ushort)~bit;
                OnTouchScreenEnd?.Invoke();
            }

            DebugText = $"{_companionState.buttons:B4}";

            _companionState.touchRadius.x = position.x;
            _companionState.touchRadius.y = position.y;

            InputSystem.QueueStateEvent(_inputDevice, _companionState);
        }

        public void SendTouchRadiusDeltaEvent(int phase, Vector2 delta)
        {
            InputSystem.QueueDeltaStateEvent(_inputDevice.touchRadiusDelta, delta, Time.realtimeSinceStartup);
        }

        public void SendLeftStickPositionEvent(int phase, Vector2 position)
        {
            var bit = 1 << 2;
            if (phase != 0)
            {
                _companionState.buttons |= (ushort)bit;
            }
            else
            {
                _companionState.buttons &= (ushort)~bit;
                OnLeftStickEnd?.Invoke();
            }

            DebugText = $"{_companionState.buttons:B4}";

            _companionState.leftStickPosition.x = position.x;
            _companionState.leftStickPosition.y = position.y;

            InputSystem.QueueStateEvent(_inputDevice, _companionState);
        }

        public void SendLeftStickDeltaEvent(int phase, Vector2 delta)
        {
            InputSystem.QueueDeltaStateEvent(_inputDevice.leftStickDelta, delta, Time.realtimeSinceStartup);
        }

        public void SendRightStickPositionEvent(int phase, Vector2 position)
        {
            var bit = 1 << 3;
            if (phase != 0)
            {
                _companionState.buttons |= (ushort)bit;
            }
            else
            {
                _companionState.buttons &= (ushort)~bit;
                OnRightStickEnd?.Invoke();
            }

            DebugText = $"{_companionState.buttons:B4}";

            _companionState.rightStickPosition.x = position.x;
            _companionState.rightStickPosition.y = position.y;

            InputSystem.QueueStateEvent(_inputDevice, _companionState);
        }

        public void SendRightStickDeltaEvent(int phase, Vector2 delta)
        {
            InputSystem.QueueDeltaStateEvent(_inputDevice.rightStickDelta, delta, Time.realtimeSinceStartup);
        }

        public void SendTouchScreenPressEvent(int i, TouchPhase contact)
        {
            var bit = (byte)contact << 1;
            if ((byte)contact < 3)
            {
                _companionState.buttons |= (ushort)bit;
            }
            else
            {
                _companionState.buttons &= (ushort)~bit;
                OnTouchScreenEnd?.Invoke();
            }

            InputSystem.QueueStateEvent(_inputDevice, _companionState);

        }
    }
}