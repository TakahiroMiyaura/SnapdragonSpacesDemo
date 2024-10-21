// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System.Collections.Generic;
using System.Linq;
using Qualcomm.Snapdragon.Spaces;
using Reseul.Snapdragon.Spaces.Controllers;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Assets.Reseul.MobileStickController.Scripts
{
    [RequireComponent(typeof(PlayerInput))]
    public class TouchScreenConvertUnityEventHandler : MonoBehaviour
    {
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
 
        private void Start()
        {
            var spacesHostView = FindObjectOfType<SpacesHostView>(true);
            if (spacesHostView != null) _phoneCamera = spacesHostView.phoneCamera;
            _playerInput = GetComponent<PlayerInput>();
        }


        void OnEnable()
        {
            if (_touchScreenIgnoreActionOnExecuting != null)
            {
                foreach (var inputActionReference in _touchScreenIgnoreActionOnExecuting)
                {
                    inputActionReference.action.started += OnPlayerInputInActive;
                    inputActionReference.action.performed += OnPlayerInputInActive;
                    inputActionReference.action.canceled += OnPlayerInputActive;
                }
            }
        }

        void OnDisable()
        {
            if (_touchScreenIgnoreActionOnExecuting != null)
            {
                foreach (var inputActionReference in _touchScreenIgnoreActionOnExecuting)
                {
                    inputActionReference.action.started -= OnPlayerInputInActive;
                    inputActionReference.action.performed -= OnPlayerInputInActive;
                    inputActionReference.action.canceled -= OnPlayerInputActive;
                }
            }
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
            var touchState = context.ReadValue<TouchState>();
            CanvasController.Instance.SendTouchScreenPositionEvent((int)TouchPhase.Began, touchState.position);
        }

        public void OnPrimaryDelta(InputAction.CallbackContext context)
        {
            var delta = context.ReadValue<Vector2>();

            CanvasController.Instance.SendTouchScreenDeltaEvent((int)TouchPhase.Moved, delta);
        }

        public void OnPrimaryPosition(InputAction.CallbackContext context)
        {
            var position = context.ReadValue<Vector2>();

            CanvasController.Instance.SendTouchScreenPositionEvent((int)TouchPhase.Began, position);
        }

        public void OnPrimaryContact(InputAction.CallbackContext context)
        {
            var contact = context.ReadValue<TouchPhase>();

            CanvasController.Instance.SendTouchScreenPressEvent((int)contact);
        }

        public void OnPrimaryRadius(InputAction.CallbackContext context)
        {
            var current = context.ReadValue<Vector2>();

            CanvasController.Instance.SendTouchRadiusEvent((int)TouchPhase.Began, _radius);

            if (!float.IsNegativeInfinity(_radius.x))
            {
                var delta = current - _radius;
                CanvasController.Instance.SendTouchRadiusDeltaEvent((int)TouchPhase.Moved, delta);
            }

            _radius = current;
        }
    }
}