// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Reseul.Snapdragon.Spaces;
using Reseul.Snapdragon.Spaces.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugVisuals : MonoBehaviour
{
    [SerializeField]
    private CameraControl _cameraControl;

    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private CanvasController _canvasController;

    private Vector2 _touchScreenData;
    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI leftStickText;
    public TextMeshProUGUI rightStickText;

    public InputActionReference touchScreen;
    public InputActionReference touchScreenDelta;

    public TextMeshProUGUI touchText;


    // Start is called before the first frame update
    private void Start()
    {
        touchScreen.action.started += ctx => _touchScreenData = ctx.ReadValue<Vector2>();
        touchScreenDelta.action.performed += ctx => _touchScreenData += ctx.ReadValue<Vector2>();
    }

    // Update is called once per frame
    private void Update()
    {
        touchText.text = $"({_touchScreenData.x:F1},{_touchScreenData.y:F1})";
        rightStickText.text = _playerController?.DebugText;
        leftStickText.text = _cameraControl?.DebugText;
        button1Text.text = _canvasController.DebugText;
    }
}