// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Qualcomm.Snapdragon.Spaces;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class OnScreenTouchVisuals : MonoBehaviour
    {
        [SerializeField]
        private Camera arCamera;

        [SerializeField]
        private RectTransform mobileStickControllerCanvas;

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

        private Vector2 currentPos = Vector2.zero;
        private TextMeshPro debugText;

        private void OnEnable()
        {
            arCamera = FindAnyObjectByType<SpacesHostView>()?.phoneCamera;
            debugText = CubeOnViewPort.GetComponentInChildren<TextMeshPro>();
            touchScreenDelta.action.performed += OnMove;
            touchScreenDelta.action.Enable();
            touchScreen.action.performed += OnStarted;
            touchScreen.action.Enable();
        }

        private void OnDisable()
        {
            touchScreenDelta.action.performed -= OnMove;
            touchScreenDelta.action.Disable();
            touchScreen.action.performed -= OnStarted;
            touchScreen.action.Disable();
        }

        private void OnStarted(InputAction.CallbackContext obj)
        {
            currentPos = obj.ReadValue<Vector2>();
            if(debugText != null) debugText.text = $"({currentPos.x:F2},{currentPos.y:F2})";
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            currentPos += obj.ReadValue<Vector2>();
            if (debugText != null) debugText.text = $"({currentPos.x:f2},{currentPos.y:f2})";

            //TouchScreen上のタップ座標にカーソルを出す
            debugCursor.position = currentPos;

            //TouchScreen上のタップ座標をワールド座標(カメラの画角内)に変換してオブジェクトを表示
            RectTransformUtility.ScreenPointToWorldPointInRectangle(mobileStickControllerCanvas, currentPos, arCamera, out var result);
            CubeOnViewPort.position = result;

            //TouchScreen上のタップ座標をワールド座標（OverlayしているCanvas上）に変換してオブジェクトを表示
            RectTransformUtility.ScreenPointToWorldPointInRectangle(mobileStickControllerCanvas, currentPos, null, out result);
            CubeOnStickControllerCanvas.position = result;

        }
    }
}