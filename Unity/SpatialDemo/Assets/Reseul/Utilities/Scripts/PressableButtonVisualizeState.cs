// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;

namespace Reseul.Snapdragon.Spaces.Utilities
{
    [RequireComponent(typeof(PressableButton))]
    public class PressableButtonVisualizeState : MonoBehaviour
    {
        [SerializeField]
        private Color inactiveColor;

        [SerializeField]
        private TextMeshProUGUI[] texts;

        private Color[] defaultColors;

        private PressableButton pressableBrFutton;

        private void Awake()
        {
            pressableBrFutton = GetComponent<PressableButton>();
            defaultColors = new Color[texts.Length];
            for (var i = 0; i < texts.Length; i++)
            {
                defaultColors[i] = texts[i].color;
            }
        }

        // Start is called before the first frame update
        private void Start()
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
            pressableBrFutton.enabled = active;
            if (active)
            {
                for (var i = 0; i < texts.Length; i++)
                {
                    texts[i].color = defaultColors[i];
                }
            }
            else
            {
                foreach (var text in texts)
                {
                    text.color = inactiveColor;
                }
            }
        }
    }
}