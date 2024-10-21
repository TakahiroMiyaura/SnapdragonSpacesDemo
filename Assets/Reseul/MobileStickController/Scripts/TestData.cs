using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestData : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _action;

    [SerializeField]
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        _action.action.performed += ctx => _text.text = $"({ctx.ReadValue<Vector2>().x:F2},{ctx.ReadValue<Vector2>().y:F2})";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
