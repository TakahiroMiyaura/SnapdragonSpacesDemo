// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using MixedReality.Toolkit.UX;
using Qualcomm.Snapdragon.Spaces;
using UnityEditor;
using UnityEngine;

namespace Reseul.Snapdragon.Spaces.Utilities
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        private DialogPool dialogPool;

        // Start is called before the first frame update
        private void Start()
        {
        }

        public void ApplicationQuit()
        {
            var dialog = dialogPool.Get();
            dialog.SetHeader("Quit");
            dialog.SetBody("Do you really want to exit this applications?");
            dialog.SetNegative("OK", args => Quit());
            dialog.SetPositive("Cancel", args => dialog.Dismiss());
            dialog.SetNeutral(null);
            dialog.ShowAsync();
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}