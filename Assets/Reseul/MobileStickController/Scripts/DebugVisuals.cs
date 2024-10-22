// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Reseul.Snapdragon.Spaces.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugVisuals : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _button1Press;
    
    [SerializeField]
    private InputActionReference _leftStick;


    [SerializeField]
    private InputActionReference _leftStickPress;

    [SerializeField]
    private InputActionReference _rightStick;


    [SerializeField]
    private InputActionReference _rightStickPress;


    [SerializeField]
    private InputActionReference _touchScreen;

    [SerializeField]
    private InputActionReference _touchScreen3D;

    [SerializeField]
    private InputActionReference _touchScreenDelta;

    [SerializeField]
    private InputActionReference _touchScreenPress;

    public TextMeshProUGUI button1PressText;
    public TextMeshProUGUI leftStickPressText;
    public TextMeshProUGUI leftStickText;
    public TextMeshProUGUI rightStickPressText;
    public TextMeshProUGUI rightStickText;
    public TextMeshProUGUI touch3DText;
    public TextMeshProUGUI touchDeltaText;
    public TextMeshProUGUI touchScreenPressText;
    public TextMeshProUGUI touchText;

    // Start is called before the first frame update
    private void Start()
    {
        _rightStick.action.performed += ctx =>
            rightStickText.text = $"({ctx.ReadValue<Vector2>().x:F2},{ctx.ReadValue<Vector2>().y:F2})";
        _rightStick.action.canceled += ctx => rightStickText.text = "(0.00,0.00)";
        _leftStick.action.performed += ctx =>
            leftStickText.text = $"({ctx.ReadValue<Vector2>().x:F2},{ctx.ReadValue<Vector2>().y:F2})";
        _leftStick.action.canceled += ctx => leftStickText.text = "(0.00,0.00)";
        _rightStickPress.action.performed += ctx => rightStickPressText.text = $"{ctx.ReadValue<float>():F2}";
        _rightStickPress.action.canceled += ctx => rightStickPressText.text = "0.0";
        _leftStickPress.action.performed += ctx => leftStickPressText.text = $"{ctx.ReadValue<float>():F2}";
        _leftStickPress.action.canceled += ctx => leftStickPressText.text = "0.0";
        _button1Press.action.performed += ctx => button1PressText.text = $"{ctx.ReadValue<float>():F2}";
        _button1Press.action.canceled += ctx => button1PressText.text = "0.0";
        _touchScreen.action.performed += ctx =>
            touchText.text = $"({ctx.ReadValue<Vector2>().x:F2},{ctx.ReadValue<Vector2>().y:F2})";
        _touchScreen.action.canceled += ctx => touchText.text = "(0.00,0.00)";
        _touchScreenPress.action.performed += ctx => touchScreenPressText.text = $"{ctx.ReadValue<float>():F2}";
        _touchScreenPress.action.canceled += ctx => touchScreenPressText.text = "0.0";
        _touchScreen3D.action.performed += ctx =>
            touch3DText.text =
                $"({ctx.ReadValue<Vector3>().x:F2},{ctx.ReadValue<Vector3>().y:F2},{ctx.ReadValue<Vector3>().z:F2})";
        _touchScreen3D.action.canceled += ctx => touch3DText.text = "(0.00,0.00,0.00)";
        _touchScreenDelta.action.performed += ctx =>
            touchDeltaText.text = $"({ctx.ReadValue<Vector2>().x:F2},{ctx.ReadValue<Vector2>().y:F2})";
        _touchScreenDelta.action.canceled += ctx => touchDeltaText.text = "(0.00,0.00)";
    }

    private void OnEnable()
    {
        _rightStick.action.Enable();
        _leftStick.action.Enable();
        _rightStickPress.action.Enable();
        _leftStickPress.action.Enable();
        _touchScreen.action.Enable();
        _touchScreenPress.action.Enable();
        _button1Press.action.Enable();
        _touchScreenPress.action.Enable();
        _touchScreen3D.action.Enable();
        _touchScreenDelta.action.Enable();
    }

    private void OnDisable()
    {
        _rightStick.action.Disable();
        _leftStick.action.Disable();
        _rightStickPress.action.Disable();
        _leftStickPress.action.Disable();
        _touchScreen.action.Disable();
        _touchScreenPress.action.Disable();
        _button1Press.action.Disable();
        _touchScreenPress.action.Disable();
        _touchScreen3D.action.Disable();
        _touchScreenDelta.action.Disable();
    }
}