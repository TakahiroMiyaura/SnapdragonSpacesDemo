// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using MixedReality.Toolkit.UX;
using Qualcomm.Snapdragon.Spaces;
using UnityEditor;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public DialogPool DialogPool;

    [SerializeField]
    private DynamicOpenXRLoader loader;

    // Start is called before the first frame update
    private void Start()
    {
    }

    public void ApplicationQuit()
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

    public void SetAutoStartOnDisplayConnectedButton(PressableButton button)
    {
        button.ForceSetToggled(SystemSettings.Instance.AutoStartOnDisplayConnected);
    }

    public void SetAutoStartOnDisplayConnected(bool value)
    {
        loader.AutoStartXROnDisplayConnected = value;
        SystemSettings.Instance.AutoStartOnDisplayConnected = value;
    }

    public void SetAutoManageXRCameraButton(PressableButton button)
    {
        button.ForceSetToggled(SystemSettings.Instance.AutoManageXRCamera);
    }

    public void SetAutoManageXRCamera(bool value)
    {
        loader.AutoManageXRCamera = value;
        SystemSettings.Instance.AutoManageXRCamera = value;
    }
}