using System.Collections;
using System.Collections.Generic;
using MixedReality.Toolkit.UX;
using TMPro;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(PressableButton))]
public class PressableButtonVisualizeState : MonoBehaviour
{
    private PressableButton _pressableBrFutton;

    [SerializeField]
    private TextMeshProUGUI[] _texts;

    [SerializeField]
    private Color _inactiveColor;

    private Color[] _defaultColors;
    void Awake()
    {
        _pressableBrFutton = GetComponent<PressableButton>();
        _defaultColors = new Color[_texts.Length];
        for (int i = 0; i < _texts.Length; i++)
        {
            _defaultColors[i] = _texts[i].color;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetVisualStateActive()
    {
        SwitchVisualState(true);
    }

    public void SetVisualStateInactive()
    {
        SwitchVisualState(false);
    }

    private void SwitchVisualState(bool active)
    {
        _pressableBrFutton.enabled = active;
        if (active)
        {
            for (int i = 0; i < _texts.Length; i++)
            {
                _texts[i].color = _defaultColors[i];
            }
        }
        else
        {
            foreach (var text in _texts)
            {
                text.color = _inactiveColor;
            }
        }
    }
}
