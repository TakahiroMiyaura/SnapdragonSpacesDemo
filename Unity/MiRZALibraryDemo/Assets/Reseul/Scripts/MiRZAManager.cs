// Copyright (c) 2025 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using System.Threading;
using com.nttqonoq.devices.android.mirzalibrary;
using TMPro;
using UnityEngine;

public class MiRZAManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI batteryLevelObj;

    [SerializeField]
    private TextMeshProUGUI chargeStatusObj;

    [SerializeField]
    private TextMeshProUGUI glassStatusObj;

    [SerializeField]
    private TextMeshProUGUI spacesModeStatusObj;

    private SynchronizationContext mainThreadContext;
    private MirzaPlugin mirzaPlugin;

    // Start is called before the first frame update
    private void Start()
    {
        mainThreadContext = SynchronizationContext.Current;

#if UNITY_ANDROID && !UNITY_EDITOR
            // ƒ‰ƒCƒuƒ‰ƒŠ‰Šú‰»
            mirzaPlugin = new MirzaPlugin();
#endif
        mirzaPlugin?.SetLogEnable(true);
    }

    public void OnMonitoringStart()
    {
        if (mirzaPlugin == null)
        {
            return;
        }

        mirzaPlugin.OnBatteryLevelChanged += OnBatteryLevelChanged;
        mirzaPlugin.OnDisplayStatusChanged += OnDisplayStatusChanged;
        mirzaPlugin.OnGlassStatusChanged += OnGlassStatusChanged;
        mirzaPlugin.OnGlassTouchGestureStatusChanged += OnGlassTouchGestureStatusChanged;
        mirzaPlugin.OnPowerOffChanged += OnPowerOffChanged;
        mirzaPlugin.OnServiceStateChanged += OnServiceStateChanged;
        mirzaPlugin.OnSpacesModeStatusChanged += OnSpacesModeStatusChanged;

        mirzaPlugin.StartMonitoring();
    }

    public void OnMonitoringStop()
    {
        if (mirzaPlugin == null)
        {
            return;
        }

        mirzaPlugin.StopMonitoring();

        mirzaPlugin.OnBatteryLevelChanged -= OnBatteryLevelChanged;
        mirzaPlugin.OnDisplayStatusChanged -= OnDisplayStatusChanged;
        mirzaPlugin.OnGlassStatusChanged -= OnGlassStatusChanged;
        mirzaPlugin.OnGlassTouchGestureStatusChanged -= OnGlassTouchGestureStatusChanged;
        mirzaPlugin.OnPowerOffChanged -= OnPowerOffChanged;
        mirzaPlugin.OnServiceStateChanged -= OnServiceStateChanged;
        mirzaPlugin.OnSpacesModeStatusChanged -= OnSpacesModeStatusChanged;
    }

    private void OnSpacesModeStatusChanged(SpacesModeStatus obj)
    {
        throw new NotImplementedException();
    }

    private void OnServiceStateChanged(ServiceState obj)
    {
        throw new NotImplementedException();
    }

    private void OnPowerOffChanged()
    {
        throw new NotImplementedException();
    }

    private void OnGlassTouchGestureStatusChanged(GlassTouchGestureStatus obj)
    {
        throw new NotImplementedException();
    }

    private void OnGlassStatusChanged(GlassStatus obj)
    {
        throw new NotImplementedException();
    }

    private void OnDisplayStatusChanged(DisplayStatus obj)
    {
        throw new NotImplementedException();
    }

    private void OnBatteryLevelChanged(int obj)
    {
        throw new NotImplementedException();
    }

    public async void GetMiRZAStatus()
    {
        mirzaPlugin.GetBatteryLevelAsync(x => PostMessage(batteryLevelObj, x.Data.ToString()));
        mirzaPlugin.GetChargeStatusAsync(x => PostMessage(chargeStatusObj, x.Data.ToString()));
        mirzaPlugin.GetGlassStatusAsync(x => PostMessage(glassStatusObj, x.Data.ToString()));
        mirzaPlugin.GetSpacesModeStatusAsync(x => PostMessage(spacesModeStatusObj, x.Data.ToString()));
    }

    private void PostMessage(TextMeshProUGUI targetText, string text)
    {
        mainThreadContext.Post(_ => { targetText.text = text; }, null);
    }
}