// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using Qualcomm.Snapdragon.Spaces;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class DisplayDebugInfoOfTouchScreen : MonoBehaviour
    {
        [SerializeField]
        private Camera arCamera;
        
        [SerializeField]
        private Transform CubeOnViewPort;
        
        [SerializeField]
        private RectTransform debugCursor;

        [SerializeField]
        private InputActionReference touchScreen;

        [SerializeField]
        private InputActionReference touchScreenDelta;

        [SerializeField]
        private InputActionReference touchScreen3D;
        
        private Vector2 currentPos = Vector2.zero;
        private TextMeshPro viewPortDebugText;
        void OnEnable()
        {
            arCamera = FindAnyObjectByType<SpacesHostView>()?.phoneCamera;
            viewPortDebugText = CubeOnViewPort.GetComponentInChildren<TextMeshPro>();
            touchScreenDelta.action.performed += OnTouchScreenDeltaPerformed;
            touchScreen.action.performed += OnTouchScreenStarted;
            touchScreen3D.action.performed += OnTouchScreen3DPerformed;
        }
        void OnDisable()
        {
            touchScreenDelta.action.performed -= OnTouchScreenDeltaPerformed;
            touchScreen.action.performed -= OnTouchScreenStarted;
            touchScreen3D.action.performed -= OnTouchScreen3DPerformed;
        }

        private void OnTouchScreenStarted(InputAction.CallbackContext obj)
        {
            currentPos = obj.ReadValue<Vector2>();
            if(viewPortDebugText != null) viewPortDebugText.text = $"({currentPos.x:F2},{currentPos.y:F2})";
        }

        private void OnTouchScreenDeltaPerformed(InputAction.CallbackContext obj)
        {
            currentPos += obj.ReadValue<Vector2>();
            if (viewPortDebugText != null) viewPortDebugText.text = $"({currentPos.x:f2},{currentPos.y:f2})";

            //TouchScreen上のタップ座標にカーソルを出す
            debugCursor.position = currentPos;
        }

        private void OnTouchScreen3DPerformed(InputAction.CallbackContext obj)
        {
            var pos = obj.ReadValue<Vector3>();
            CubeOnViewPort.position = pos;
            CubeOnViewPort.transform.rotation = arCamera.transform.rotation;
        }

    }
}