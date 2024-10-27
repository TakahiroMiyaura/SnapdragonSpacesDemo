// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class DebugInfoVisuals : MonoBehaviour
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


        void Start()
        {
            rightStick.action.performed += ctx =>
                rightStickText.text = $"({ctx.ReadValue<Vector2>().x:F2},{ctx.ReadValue<Vector2>().y:F2})";
            rightStick.action.canceled += _ => rightStickText.text = "(0.00,0.00)";
            leftStick.action.performed += ctx =>
                leftStickText.text = $"({ctx.ReadValue<Vector2>().x:F2},{ctx.ReadValue<Vector2>().y:F2})";
            leftStick.action.canceled += _ => leftStickText.text = "(0.00,0.00)";
            rightStickPress.action.performed += ctx => rightStickPressText.text = $"{ctx.ReadValue<float>():F2}";
            rightStickPress.action.canceled += _ => rightStickPressText.text = "0.0";
            leftStickPress.action.performed += ctx => leftStickPressText.text = $"{ctx.ReadValue<float>():F2}";
            leftStickPress.action.canceled += _ => leftStickPressText.text = "0.0";
            button1Press.action.performed += ctx => button1PressText.text = $"{ctx.ReadValue<float>():F2}";
            button1Press.action.canceled += _ => button1PressText.text = "0.0";
            touchScreen.action.performed += ctx =>
                touchText.text = $"({ctx.ReadValue<Vector2>().x:F2},{ctx.ReadValue<Vector2>().y:F2})";
            touchScreen.action.canceled += _ => touchText.text = "(0.00,0.00)";
            touchScreenPress.action.performed += ctx => touchScreenPressText.text = $"{ctx.ReadValue<float>():F2}";
            touchScreenPress.action.canceled += _ => touchScreenPressText.text = "0.0";
            touchScreen3D.action.performed += ctx =>
                touch3DText.text =
                    $"({ctx.ReadValue<Vector3>().x:F2},{ctx.ReadValue<Vector3>().y:F2},{ctx.ReadValue<Vector3>().z:F2})";
            touchScreen3D.action.canceled += _ => touch3DText.text = "(0.00,0.00,0.00)";
            touchScreenDelta.action.performed += ctx =>
                touchDeltaText.text = $"({ctx.ReadValue<Vector2>().x:F2},{ctx.ReadValue<Vector2>().y:F2})";
            touchScreenDelta.action.canceled += _ => touchDeltaText.text = "(0.00,0.00)";
        }

        void OnEnable()
        {
            rightStick.action.Enable();
            leftStick.action.Enable();
            rightStickPress.action.Enable();
            leftStickPress.action.Enable();
            touchScreen.action.Enable();
            touchScreenPress.action.Enable();
            button1Press.action.Enable();
            touchScreenPress.action.Enable();
            touchScreen3D.action.Enable();
            touchScreenDelta.action.Enable();
        }

        void OnDisable()
        {
            rightStick.action.Disable();
            leftStick.action.Disable();
            rightStickPress.action.Disable();
            leftStickPress.action.Disable();
            touchScreen.action.Disable();
            touchScreenPress.action.Disable();
            button1Press.action.Disable();
            touchScreenPress.action.Disable();
            touchScreen3D.action.Disable();
            touchScreenDelta.action.Disable();
        }
    }
}