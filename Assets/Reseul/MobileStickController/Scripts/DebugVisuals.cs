
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugVisuals : MonoBehaviour
{

    public InputActionReference toutchpad;
    public InputActionReference rightStick;
    public InputActionReference leftStick;
    public InputActionReference button1;

    public TextMeshProUGUI touchText;
    public TextMeshProUGUI rightStickText;
    public TextMeshProUGUI leftStickText;
    public TextMeshProUGUI button1Text;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var toutchpadData = toutchpad.action.ReadValue<Vector2>();
        var rightStickData = rightStick.action.ReadValue<Vector2>();
        var leftStickData = leftStick.action.ReadValue<Vector2>();
        var button1Data = button1.action.IsPressed();

        touchText.text = $"({toutchpadData.x:F1},{toutchpadData.y:F1})";
        rightStickText.text = $"({rightStickData.x:F1},{rightStickData.y:F1})";
        leftStickText.text = $"({leftStickData.x:F1},{leftStickData.y:F1})";
        button1Text.text = $"({button1.action.name}-{button1Data})";

    }
}
