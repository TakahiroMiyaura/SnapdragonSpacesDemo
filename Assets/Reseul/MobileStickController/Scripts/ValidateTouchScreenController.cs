// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Qualcomm.Snapdragon.Spaces;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Reseul.MobileStickController.Scripts
{
    public class ValidateTouchScreenController : MonoBehaviour
    {
        [SerializeField]
        private Camera _arCamera;

        [SerializeField]
        private RectTransform _camCanvas;
        
        [SerializeField]
        private GameObject _TestObj;

        [SerializeField]
        private InputActionReference _touchScreen;

        [SerializeField]
        private InputActionReference _touchScreenDelta;

        private Vector2 currentPos = Vector2.zero;
        private TextMeshPro _debugText;

        private void OnEnable()
        {
            _arCamera = FindAnyObjectByType<SpacesHostView>()?.phoneCamera;
            _debugText = _TestObj.GetComponentInChildren<TextMeshPro>();
            _touchScreenDelta.action.performed += OnMove;
            _touchScreenDelta.action.Enable();
            _touchScreen.action.performed += OnStarted;
            _touchScreen.action.Enable();
        }

        private void OnDisable()
        {
            _touchScreenDelta.action.performed -= OnMove;
            _touchScreenDelta.action.Disable();
            _touchScreen.action.performed -= OnStarted;
            _touchScreen.action.Disable();
        }

        private void OnStarted(InputAction.CallbackContext obj)
        {
            currentPos = obj.ReadValue<Vector2>();
            if(_debugText != null) _debugText.text = $"({currentPos.x:F2},{currentPos.y:F2})";
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            currentPos += obj.ReadValue<Vector2>();
            if (_debugText != null) _debugText.text = $"({currentPos.x:f2},{currentPos.y:f2})";

            RectTransformUtility.ScreenPointToWorldPointInRectangle(_camCanvas, currentPos, _arCamera, out var result);
            _TestObj.transform.position = result;
        }
    }
}