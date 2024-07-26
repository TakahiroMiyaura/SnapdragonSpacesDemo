// Copyright (c) 2023 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using MixedReality.Toolkit.UX;
using UnityEngine;

public class DialogActionDemo : MonoBehaviour
{
    public Material Blue;
    public DialogPool.Policy Policy = DialogPool.Policy.DismissExisting;

    public DialogPool pool;
    public Material Red;

    public MeshRenderer Target;

    public void OnChangeRedColor()
    {
        var dialog = pool.Get(Policy);
        if (dialog == null) return; 
        dialog.SetHeader("Change Color?");
        dialog.SetBody("Do you want to change the color to red?");
        dialog.SetNegative("No", args => dialog.Dismiss());
        dialog.SetNeutral(null);
        dialog.SetPositive("Yes", args =>
        {
            Target.material = Red;
            dialog.Dismiss();
        });
        dialog.ShowAsync();
    }

    public void OnChangeBlueColor()
    {
        var dialog = pool.Get(Policy);
        if (dialog == null) return;
        dialog.SetHeader("Change Color?");
        dialog.SetBody("Do you want to change the color to blue?");
        dialog.SetNegative("No", args => dialog.Dismiss());
        dialog.SetNeutral(null);
        dialog.SetPositive("Yes", args =>
        {
            Target.material = Blue;
            dialog.Dismiss();
        });
        dialog.ShowAsync();
    }
}