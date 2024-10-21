// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System.Collections.Generic;
using System.Linq;
using Qualcomm.Snapdragon.Spaces;
using Reseul.Snapdragon.Spaces.Controllers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Assets.Reseul.MobileStickController.Scripts
{
    [RequireComponent(typeof(PlayerInput))]
    public class TouchScreenConvertHandler : MonoBehaviour
    {

        public TextMeshProUGUI text;
        private static object _lockObj = new object();

        [SerializeField]
        private Canvas _canvas;

        private CanvasControllerInputDeviceState _companionState;

        [SerializeField]
        private RectTransform[] _ignoreCanvasOnFirstTap;

        private Camera _phoneCamera;

        private Vector2 _radius = Vector2.negativeInfinity;
        private PlayerInput _playerInput;

        [SerializeField]
        private InputActionReference[] _touchScreenIgnoreActionOnExecuting;

        private Dictionary<System.Guid,int> _activeActions = new Dictionary<System.Guid, int>();
        void Awake()
        {

            _playerInput = GetComponent<PlayerInput>();

            var spacesHostView = FindObjectOfType<SpacesHostView>(true);
            if (spacesHostView != null) _phoneCamera = spacesHostView.phoneCamera;
        }

        void OnEnable()
        {
            if (_playerInput == null) return;

            // デリゲート登録
            _playerInput.onActionTriggered += OnMove;
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            switch (obj.action.name)
            {
                case "PrimaryTouch":
                    OnPrimaryTouch(obj);
                    break;
                case "PrimaryDelta":
                    OnPrimaryDelta(obj);
                    break;
                case "PrimaryPosition":
                    OnPrimaryPosition(obj);
                    break;
                case "PrimaryContact":
                    OnPrimaryContact(obj);
                    break;
                case "PrimaryRadius":
                    OnPrimaryRadius(obj);
                    break;
                case "SinglePress":
                    OnSinglePress(obj);
                    break;
            }
        }


        void OnDisable()
        {
            if (_playerInput == null) return;

            // デリゲート登録
            _playerInput.onActionTriggered -= OnMove;
        }

        private void OnPlayerInputActive(InputAction.CallbackContext obj)
        {
            lock (_lockObj)
            {
                _activeActions[obj.action.id] = 0;
                if (_activeActions.Values.Sum() == 0)
                {
                    _playerInput.enabled = true;
                    Debug.Log("OnPlayerInputActive");
                }
            }
        }

        private void OnPlayerInputInActive(InputAction.CallbackContext obj)
        {
            lock (_lockObj)
            {
                _activeActions[obj.action.id] = 1;
            }
            _playerInput.enabled = false;

        }

        // Touch #0 入力
        public void OnPrimaryTouch(InputAction.CallbackContext context)
        {
            text.text = "OnPrimaryTouch > " + context.phase.ToString();
            var touchState = context.ReadValue<TouchState>();
            CanvasController.Instance.SendTouchState(touchState);
            //switch (context.phase)
            //{
            //    case InputActionPhase.Started:
            //        CanvasController.Instance.SendTouchScreenPositionEvent((int)TouchPhase.Began, touchState.position);
            //        break;
            //    case InputActionPhase.Performed:
            //        CanvasController.Instance.SendTouchScreenPositionEvent((int)TouchPhase.Stationary, touchState.position);
            //        break;
            //    case InputActionPhase.Canceled:
            //        CanvasController.Instance.SendTouchScreenPositionEvent((int)TouchPhase.Ended, touchState.position);
            //        break;
            //}
        }

        public void OnPrimaryDelta(InputAction.CallbackContext context)
        {
            text.text = "OnPrimaryDelta > " + context.phase.ToString();
            var delta = context.ReadValue<Vector2>();
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    CanvasController.Instance.SendTouchScreenDeltaEvent(1, delta);
                    break;
                case InputActionPhase.Performed:
                    CanvasController.Instance.SendTouchScreenDeltaEvent(1, delta);
                    break;
                case InputActionPhase.Canceled:
                    CanvasController.Instance.SendTouchScreenDeltaEvent(0, delta);
                    break;
            }

        }

        public void OnPrimaryPosition(InputAction.CallbackContext context)
        {
            text.text = "OnPrimaryPosition > " + context.phase.ToString();
            var position = context.ReadValue<Vector2>();
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    CanvasController.Instance.SendTouchScreenPositionEvent(1, position);
                    break;
                case InputActionPhase.Performed:
                    CanvasController.Instance.SendTouchScreenPositionEvent(1, position);
                    break;
                case InputActionPhase.Canceled:
                    CanvasController.Instance.SendTouchScreenPositionEvent(0, position);
                    break;
            }
        }

        public void OnPrimaryContact(InputAction.CallbackContext context)
        {
            text.text = "OnPrimaryContact > " + context.phase.ToString();
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    CanvasController.Instance.SendTouchScreenPressEvent((int)1);
                    break;
                case InputActionPhase.Performed:
                    CanvasController.Instance.SendTouchScreenPressEvent((int)1);
                    break;
                case InputActionPhase.Canceled:
                    CanvasController.Instance.SendTouchScreenPressEvent((int)0);
                    break;
            }
        }

        public void OnSinglePress(InputAction.CallbackContext context)
        {
            text.text = "OnSinglePress > " + context.phase.ToString();
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    CanvasController.Instance.SendTouchScreenPressEvent((int)1);
                    break;
                case InputActionPhase.Performed:
                    CanvasController.Instance.SendTouchScreenPressEvent((int)1);
                    break;
                case InputActionPhase.Canceled:
                    CanvasController.Instance.SendTouchScreenPressEvent((int)0);
                    break;
            }
        }

        public void OnPrimaryRadius(InputAction.CallbackContext context)
        {
            text.text = "OnPrimaryRadius > " + context.phase.ToString();
            var current = context.ReadValue<Vector2>();
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    CanvasController.Instance.SendTouchRadiusEvent(1, current);
                    break;
                case InputActionPhase.Performed:
                    CanvasController.Instance.SendTouchRadiusEvent(1, current);
                    break;
                case InputActionPhase.Canceled:
                    CanvasController.Instance.SendTouchRadiusEvent(0, current);
                    break;
            }
        }
    }
}