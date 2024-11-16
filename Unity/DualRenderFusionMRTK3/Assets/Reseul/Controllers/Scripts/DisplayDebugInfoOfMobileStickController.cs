// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class DisplayDebugInfoOfMobileStickController : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference button1Press = null;

        [SerializeField]
        private InputActionReference leftStick = null;

        [SerializeField]
        private InputActionReference leftStickPress = null;

        [SerializeField]
        private InputActionReference rightStick = null;

        [SerializeField]
        private InputActionReference rightStickPress = null;

        [SerializeField]
        private InputActionReference touchScreen = null;

        [SerializeField]
        private InputActionReference touchScreen3D = null;
        
        [SerializeField]
        private InputActionReference touchScreenDelta = null;

        [SerializeField]
        private InputActionReference touchScreenPress = null;

        [SerializeField]
        private TextMeshProUGUI button1PressText = null;

        [SerializeField]
        private TextMeshProUGUI leftStickPressText = null;

        [SerializeField]
        private TextMeshProUGUI leftStickText = null;

        [SerializeField]
        private TextMeshProUGUI rightStickPressText = null;

        [SerializeField]
        private TextMeshProUGUI rightStickText = null;

        [SerializeField]
        private TextMeshProUGUI touch3DText = null;
        
        [SerializeField]
        private TextMeshProUGUI touchDeltaText = null;

        [SerializeField]
        private TextMeshProUGUI touchScreenPressText = null;

        [SerializeField]
        private TextMeshProUGUI touchText = null;

        void OnEnable()
        {
            button1Press.action.performed += Button1PressPerformed;
            button1Press.action.canceled += Button1PressCanceled;
            leftStickPress.action.performed += LeftStickPressPerformed;
            leftStickPress.action.canceled += LeftStickPressCanceled;
            rightStickPress.action.performed += RightStickPressPerformed;
            rightStickPress.action.canceled += RightStickPressCanceled;
            touchScreenPress.action.performed += TouchScreenPressPerformed;
            touchScreenPress.action.canceled += TouchScreenPressCanceled;
            leftStick.action.performed += LeftStickPerformed;
            leftStick.action.canceled += LeftStickCanceled;
            rightStick.action.performed += RightStickPerformed;
            rightStick.action.canceled += RightStickCanceled;
            touchScreen.action.performed += TouchScreenPerformed;
            touchScreen.action.canceled += TouchScreenCanceled;
            touchScreen3D.action.performed += TouchScreen3DPerformed;
            touchScreen3D.action.canceled += TouchScreen3DCanceled;
            touchScreenDelta.action.performed += TouchScreenDeltaPerformed;
            touchScreenDelta.action.canceled += TouchScreenDeltaCanceled;
        }
        void OnDisable()
        {
            button1Press.action.performed -= Button1PressPerformed;
            button1Press.action.canceled -= Button1PressCanceled;
            leftStickPress.action.performed -= LeftStickPressPerformed;
            leftStickPress.action.canceled -= LeftStickPressCanceled;
            rightStickPress.action.performed -= RightStickPressPerformed;
            rightStickPress.action.canceled -= RightStickPressCanceled;
            touchScreenPress.action.performed -= TouchScreenPressPerformed;
            touchScreenPress.action.canceled -= TouchScreenPressCanceled;
            leftStick.action.performed -= LeftStickPerformed;
            leftStick.action.canceled -= LeftStickCanceled;
            rightStick.action.performed -= RightStickPerformed;
            rightStick.action.canceled -= RightStickCanceled;
            touchScreen.action.performed -= TouchScreenPerformed;
            touchScreen.action.canceled -= TouchScreenCanceled;
            touchScreen3D.action.performed -= TouchScreen3DPerformed;
            touchScreen3D.action.canceled -= TouchScreen3DCanceled;
            touchScreenDelta.action.performed -= TouchScreenDeltaPerformed;
            touchScreenDelta.action.canceled -= TouchScreenDeltaCanceled;
        }

        private void Button1PressPerformed(InputAction.CallbackContext ctx)
        {
            button1PressText.text = $"{ctx.ReadValue<float>():F2}";
        }

        private void Button1PressCanceled(InputAction.CallbackContext ctx)
        {
            button1PressText.text = "0.0";
        }

        private void LeftStickPressPerformed(InputAction.CallbackContext ctx)
        {
            leftStickPressText.text = $"{ctx.ReadValue<float>():F2}";
        }

        private void LeftStickPressCanceled(InputAction.CallbackContext ctx)
        {
            leftStickPressText.text = "0.0";
        }

        private void RightStickPressPerformed(InputAction.CallbackContext ctx)
        {
            rightStickPressText.text = $"{ctx.ReadValue<float>():F2}";
        }

        private void RightStickPressCanceled(InputAction.CallbackContext ctx)
        {
            rightStickPressText.text = "0.0";
        }

        private void TouchScreenPressPerformed(InputAction.CallbackContext ctx)
        {
            touchScreenPressText.text = $"{ctx.ReadValue<float>():F2}";
        }

        private void TouchScreenPressCanceled(InputAction.CallbackContext ctx)
        {
            touchScreenPressText.text = "0.0";
        }

        private void RightStickPerformed(InputAction.CallbackContext ctx)
        {
            rightStickText.text = $"({ctx.ReadValue<Vector2>().x:F2},{ctx.ReadValue<Vector2>().y:F2})";
        }
        private void RightStickCanceled(InputAction.CallbackContext ctx)
        {
            rightStickText.text = "(0.00,0.00)";
        }

        private void LeftStickPerformed(InputAction.CallbackContext ctx)
        {
            leftStickText.text = $"({ctx.ReadValue<Vector2>().x:F2},{ctx.ReadValue<Vector2>().y:F2})";
        }
        private void LeftStickCanceled(InputAction.CallbackContext ctx)
        {
            leftStickText.text = "(0.00,0.00)";
        }

        private void TouchScreenPerformed(InputAction.CallbackContext ctx)
        {
            touchText.text = $"({ctx.ReadValue<Vector2>().x:F2},{ctx.ReadValue<Vector2>().y:F2})";
        }

        private void TouchScreenCanceled(InputAction.CallbackContext ctx)
        {
            touchText.text = "(0.00,0.00)";
        }

        private void TouchScreen3DPerformed(InputAction.CallbackContext ctx)
        {
            touch3DText.text =
                $"({ctx.ReadValue<Vector3>().x:F2},{ctx.ReadValue<Vector3>().y:F2},{ctx.ReadValue<Vector3>().z:F2})";
        }

        private void TouchScreen3DCanceled(InputAction.CallbackContext ctx)
        {
            touch3DText.text = "(0.00,0.00,0.00)";
        }
        
        private void TouchScreenDeltaPerformed(InputAction.CallbackContext ctx)
        {
            touchDeltaText.text = $"({ctx.ReadValue<Vector2>().x:F2},{ctx.ReadValue<Vector2>().y:F2})";
        }

        private void TouchScreenDeltaCanceled(InputAction.CallbackContext ctx)
        {
            touchDeltaText.text = "(0.00,0.00)";
        }
    }
}