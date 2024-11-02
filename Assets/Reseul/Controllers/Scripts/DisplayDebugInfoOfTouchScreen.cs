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
        private RectTransform mobileStickControllerCanvas;
        [SerializeField]
        private RectTransform mobileStickControllerCanvas2;

        [SerializeField]
        private Transform CubeOnViewPort;


        [SerializeField]
        private Transform CubeOnStickControllerCanvas;

        [SerializeField]
        private RectTransform debugCursor;

        [SerializeField]
        private InputActionReference touchScreen;

        [SerializeField]
        private InputActionReference touchScreenDelta;

        [SerializeField]
        private InputActionReference touchScreen3D;

        [SerializeField]
        private InputActionReference touchScreen3DOnCanvas;

        private Vector2 currentPos = Vector2.zero;
        private TextMeshPro viewPortebugText;

        private void OnEnable()
        {
            arCamera = FindAnyObjectByType<SpacesHostView>()?.phoneCamera;
            viewPortebugText = CubeOnViewPort.GetComponentInChildren<TextMeshPro>();
            touchScreenDelta.action.performed += OnTouchScreenDeltaPerformed;
            touchScreenDelta.action.Enable();
            touchScreen.action.performed += OnTouchScreenStarted;
            touchScreen.action.Enable();
            touchScreen3D.action.performed += OnTouchScreen3DPerformed;
            touchScreen3D.action.Enable();
            touchScreen3DOnCanvas.action.performed += OnTouchScreen3DOnCanvasPerformed;
            touchScreen3DOnCanvas.action.Enable();
        }

        private void OnDisable()
        {
            touchScreenDelta.action.performed -= OnTouchScreenDeltaPerformed;
            touchScreenDelta.action.Disable();
            touchScreen.action.performed -= OnTouchScreenStarted;
            touchScreen.action.Disable();
            touchScreen.action.performed -= OnTouchScreenStarted;
            touchScreen.action.Disable();
            touchScreen3D.action.performed -= OnTouchScreen3DPerformed;
            touchScreen3D.action.Disable();
            touchScreen3DOnCanvas.action.performed -= OnTouchScreen3DOnCanvasPerformed;
            touchScreen3DOnCanvas.action.Disable();
        }

        private void OnTouchScreenStarted(InputAction.CallbackContext obj)
        {
            currentPos = obj.ReadValue<Vector2>();
            if(viewPortebugText != null) viewPortebugText.text = $"({currentPos.x:F2},{currentPos.y:F2})";
        }

        private void OnTouchScreenDeltaPerformed(InputAction.CallbackContext obj)
        {
            currentPos += obj.ReadValue<Vector2>();
            if (viewPortebugText != null) viewPortebugText.text = $"({currentPos.x:f2},{currentPos.y:f2})";

            //TouchScreen上のタップ座標にカーソルを出す
            debugCursor.position = currentPos;

            //TouchScreen上のタップ座標をワールド座標(カメラの画角内)に変換してオブジェクトを表示
            //RectTransformUtility.ScreenPointToWorldPointInRectangle(mobileStickControllerCanvas2, currentPos, arCamera, out var result);
            //CubeOnViewPort.position = result;
            //CubeOnViewPort.transform.rotation = arCamera.transform.rotation;

            //TouchScreen上のタップ座標をワールド座標（OverlayしているCanvas上）に変換してオブジェクトを表示
            //RectTransformUtility.ScreenPointToWorldPointInRectangle(mobileStickControllerCanvas, currentPos, null, out result);
            //CubeOnStickControllerCanvas.position = result;

        }

        private void OnTouchScreen3DPerformed(InputAction.CallbackContext obj)
        {
            var pos = obj.ReadValue<Vector3>();
            CubeOnViewPort.position = pos;
            CubeOnViewPort.transform.rotation = arCamera.transform.rotation;
        }

        private void OnTouchScreen3DOnCanvasPerformed(InputAction.CallbackContext obj)
        {
            var pos = obj.ReadValue<Vector3>();
            CubeOnStickControllerCanvas.position = pos;
        }
    }
}