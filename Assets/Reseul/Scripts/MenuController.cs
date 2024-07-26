// Copyright (c) 2023 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using MixedReality.Toolkit.UX;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class MenuController : MonoBehaviour
{
    public DialogPool DialogPool;

    public UnityEvent OnPositive;

    // Start is called before the first frame update
    private void Start()
    {
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
}