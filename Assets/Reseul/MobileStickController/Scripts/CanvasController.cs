// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Debug = UnityEngine.Debug;
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

        public UnityEvent OnLeftStickStart;
        public UnityEvent OnRightStickStart;
        public UnityEvent OnTouchScreenStart;

        private bool _isLeftStickActive;
        private bool _isRightStickActive;
        private bool _isTouchScreenActive;


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
            var bit = phase << 0;
            if (phase != 0)
                _companionState.NewButtons |= (ushort)bit;
            else
                _companionState.NewButtons &= (ushort)~bit;
            DebugText = Convert.ToString(_companionState.NewButtons, 2);
            Debug.Log(DebugText);
            InputSystem.QueueStateEvent(_inputDevice, _companionState);
        }

        public void SendTouchScreenPositionEvent(int phase, Vector2 position)
        {
            var bit = phase << 1;
            if (phase != 0)
            {
                _companionState.NewButtons |= (ushort)bit;
                if (!_isTouchScreenActive)
                {
                    OnTouchScreenStart?.Invoke();
                    _isTouchScreenActive = true;
                }

            }
            else
            {
                _companionState.NewButtons &= (ushort)~bit;
                OnTouchScreenEnd?.Invoke();
                _isTouchScreenActive = false;
                _companionState.touchRadius = Vector2.zero;
                _companionState.touchRadiusDelta = Vector2.zero;
                _companionState.touchScreenDelta = Vector2.zero;
                _companionState.touchScreenPosition = Vector2.zero;
                _companionState.touchScreenPosition3D = Vector3.zero;
            }

            DebugText = Convert.ToString(_companionState.NewButtons, 2);
            Debug.Log(DebugText);

            _companionState.touchScreenPosition = position;

            InputSystem.QueueStateEvent(_inputDevice, _companionState);
        }

        public void SendTouchScreenDeltaEvent(int phase, Vector2 delta)
        {
            InputSystem.QueueDeltaStateEvent(_inputDevice.touchScreenDelta, delta, Time.realtimeSinceStartup);
        }

        public void SendTouchScreenPosition3DEvent(int phase, Vector3 normalizedPosition)
        {
            var bit = phase << 1;
            if (phase != 0)
            {
                _companionState.NewButtons |= (ushort)bit;
                if (!_isTouchScreenActive)
                {
                    OnTouchScreenStart?.Invoke();
                    _isTouchScreenActive = true;
                }
            }
            else
            {
                _companionState.NewButtons &= (ushort)~bit;
                OnTouchScreenEnd?.Invoke();
                _isTouchScreenActive = false;
                _companionState.touchRadius = Vector2.zero;
                _companionState.touchRadiusDelta = Vector2.zero;
                _companionState.touchScreenDelta = Vector2.zero;
                _companionState.touchScreenPosition = Vector2.zero;
                _companionState.touchScreenPosition3D = Vector3.zero;
            }

            DebugText = Convert.ToString(_companionState.NewButtons, 2);
            Debug.Log(DebugText);

            _companionState.touchScreenPosition3D = normalizedPosition;

            InputSystem.QueueStateEvent(_inputDevice, _companionState);
        }

        public void SendTouchRadiusEvent(int phase, Vector2 position)
        {
            var bit = phase << 1;
            if (phase != 0)
            {
                _companionState.NewButtons |= (ushort)bit;
                if (!_isTouchScreenActive)
                {
                    OnTouchScreenStart?.Invoke();
                    _isTouchScreenActive = true;
                }
            }
            else
            {
                _companionState.NewButtons &= (ushort)~bit;
                OnTouchScreenEnd?.Invoke();
                _isTouchScreenActive = false;
                _companionState.touchRadius = Vector2.zero;
                _companionState.touchRadiusDelta = Vector2.zero;
                _companionState.touchScreenDelta = Vector2.zero;
                _companionState.touchScreenPosition = Vector2.zero;
                _companionState.touchScreenPosition3D = Vector3.zero;
            }

            DebugText = Convert.ToString(_companionState.NewButtons, 2);
            Debug.Log(DebugText);

            _companionState.touchRadius = position;

            InputSystem.QueueStateEvent(_inputDevice, _companionState);
        }

        public void SendTouchRadiusDeltaEvent(int phase, Vector2 delta)
        {
            InputSystem.QueueDeltaStateEvent(_inputDevice.touchRadiusDelta, delta, Time.realtimeSinceStartup);
        }

        public void SendLeftStickPositionEvent(int phase, Vector2 position)
        {
            var bit = phase << 2;
            if (phase != 0)
            {
                _companionState.NewButtons |= (ushort)bit;
                if (!_isLeftStickActive)
                {
                    OnLeftStickStart?.Invoke();
                    _isLeftStickActive = true;
                }
            }
            else
            {
                _companionState.NewButtons &= (ushort)~bit;
                OnLeftStickEnd?.Invoke();
                _isLeftStickActive = false;
            }

            DebugText = Convert.ToString(_companionState.NewButtons, 2);
            Debug.Log(DebugText);

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
            var bit = phase << 3;
            if (phase != 0)
            {
                _companionState.NewButtons |= (ushort)bit;
                if (!_isRightStickActive)
                {
                    OnRightStickStart?.Invoke();
                    _isRightStickActive = true;
                }
            }
            else
            {
                _companionState.NewButtons &= (ushort)~bit;
                OnRightStickEnd?.Invoke();
                _isRightStickActive = false;
            }

            DebugText = Convert.ToString(_companionState.NewButtons, 2);
            Debug.Log(DebugText);

            _companionState.rightStickPosition.x = position.x;
            _companionState.rightStickPosition.y = position.y;

            InputSystem.QueueStateEvent(_inputDevice, _companionState);
        }

        public void SendRightStickDeltaEvent(int phase, Vector2 delta)
        {
            InputSystem.QueueDeltaStateEvent(_inputDevice.rightStickDelta, delta, Time.realtimeSinceStartup);
        }

        public void SendTouchScreenPressEvent(int phase)
        {
            var bit = (byte)phase << 1;
            if (phase != 0)
            {
                _companionState.NewButtons |= (ushort)bit;
                if (!_isTouchScreenActive)
                {
                    OnTouchScreenStart?.Invoke();
                    _isTouchScreenActive = true;
                }
            }
            else
            {
                _companionState.NewButtons &= (ushort)~bit;
                OnTouchScreenEnd?.Invoke();
                _isTouchScreenActive = false;
                _companionState.touchRadius = Vector2.zero;
                _companionState.touchRadiusDelta = Vector2.zero;
                _companionState.touchScreenDelta = Vector2.zero;
                _companionState.touchScreenPosition = Vector2.zero;
                _companionState.touchScreenPosition3D = Vector3.zero;
            }

            InputSystem.QueueStateEvent(_inputDevice, _companionState);

        }

        public void SendTouchState(TouchState state)
        {
            _companionState.NewTouchState = state;
            InputSystem.QueueStateEvent(_inputDevice, _companionState);
        }
    }
}