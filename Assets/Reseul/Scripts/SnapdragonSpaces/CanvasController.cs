// Copyright (c) 2023 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class CanvasController : MonoBehaviour
    {

        private static CanvasController instanceObj;
        public TextMeshProUGUI text;

        public static CanvasController Instance
        {
            get
            {
                if (instanceObj == null)
                {
                    instanceObj = FindObjectOfType<CanvasController>();
                }
                return instanceObj;
            }
        }
        
        CanvasControllerInputDeviceState companionState;

        void Awake()
        {
            companionState = new CanvasControllerInputDeviceState();
            companionState.trackingState = 1;
        }

        void Start()
        {
        }

        public void ReloadApp()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        public void SendButton1Event(int phase)
        {
            var bit = 1 << (int)0;
            if (phase != 0)
            {
                companionState.buttons |= (ushort)bit;
            }
            else
            {
                companionState.buttons &= (ushort)~bit;
            }
            text.text = Convert.ToString(companionState.buttons, 2);
            InputSystem.QueueStateEvent(InputSystem.GetDevice<CanvasControllerInputDevice>(), companionState);
        }

        public void SendTouchpadEvent(int phase, Vector2 position)
        {
            var bit = 1 << (int)1;
            if (phase != 0)
            {
                companionState.buttons |= (ushort)bit;
            }
            else
            {
                companionState.buttons &= (ushort)~bit;
                OnTouchpadEnd?.Invoke();
            }
            text.text = Convert.ToString(companionState.buttons, 2);

            companionState.touchpadPosition.x = position.x;
            companionState.touchpadPosition.y = position.y;

            InputSystem.QueueStateEvent(InputSystem.GetDevice<CanvasControllerInputDevice>(), companionState);
        }

        public void SendLeftStickEvent(int phase, Vector2 position)
        {
            var bit = 1 << (int)2;
            if (phase != 0)
            {
                companionState.buttons |= (ushort)bit;
            }
            else
            {
                companionState.buttons &= (ushort)~bit;
                OnLeftStickEnd?.Invoke();
            }
            text.text = Convert.ToString(companionState.buttons, 2);

            companionState.leftStickPosition.x = position.x;
            companionState.leftStickPosition.y = position.y;

            InputSystem.QueueStateEvent(InputSystem.GetDevice<CanvasControllerInputDevice>(), companionState);
        }

        public void SendRightStickEvent(int phase, Vector2 position)
        {
            var bit = 1 << (int)3;
            if (phase != 0)
            {
                companionState.buttons |= (ushort)bit;
            }
            else
            {
                companionState.buttons &= (ushort)~bit;
                OnRightStickEnd?.Invoke();
            }
            text.text = Convert.ToString(companionState.buttons, 2);

            companionState.rightStickPosition.x = position.x;
            companionState.rightStickPosition.y = position.y;

            InputSystem.QueueStateEvent(InputSystem.GetDevice<CanvasControllerInputDevice>(), companionState);
        }

        public UnityEvent OnTouchpadEnd;
        public UnityEvent OnLeftStickEnd;
        public UnityEvent OnRightStickEnd;

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}