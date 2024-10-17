// Copyright (c) 2023 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System.Net.Mime;
using MixedReality.Toolkit.UX;
using Qualcomm.Snapdragon.Spaces;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class MenuController : MonoBehaviour
{
    public DialogPool DialogPool;
    
    [SerializeField]
    private DynamicOpenXRLoader _loader;

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    private void Start()
    {
        text.text = $"{Screen.currentResolution.width},{Screen.currentResolution.height}";
    }

    // Update is called once per frame
    private void Update()
    {
    }


    public void Test()
    {
        var dialog = DialogPool.Get();
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

    public void SetAutoStartOnDisplayConnected(bool value)
    {
        _loader.AutoStartXROnDisplayConnected = value;
    }

    public void SetAutoManageXRCamera(bool value)
    {
        _loader.AutoManageXRCamera = value;
    }

}