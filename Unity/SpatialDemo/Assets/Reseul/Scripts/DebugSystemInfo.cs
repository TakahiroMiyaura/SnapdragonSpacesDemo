// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using TMPro;
using UnityEngine;

namespace Reseul.Snapdragon.Spaces.Utilities
{
    public class DebugSystemInfo : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI DisplaySize;

        // Start is called before the first frame update
        private void Start()
        {
            DisplaySize.text = $"{Screen.currentResolution.width},{Screen.currentResolution.height}";
        }
    }
}