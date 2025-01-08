// Copyright (c) 2025 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System.Threading;
using com.nttqonoq.devices.android.mirzalibrary;
using UnityEngine;

public class MenuSlider : MonoBehaviour
{
    private SynchronizationContext mainThreadContext;

    // Start is called before the first frame update
    private void Start()
    {
        mainThreadContext = SynchronizationContext.Current;

        MiRZAManager.Instance.OnGlassTouchGestureStatusChanged.AddListener(RotateMenu);
    }

    private void RotateMenu(GlassTouchGestureStatus arg0)
    {
        var angle = 360f / 256f * arg0.XCoordinate;
        mainThreadContext.Post(_ => { transform.localRotation = Quaternion.Euler(0, angle, 0); }, null);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}