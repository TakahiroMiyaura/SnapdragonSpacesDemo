// Copyright (c) 2025 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using System.Text;
using System.Threading;
using com.nttqonoq.devices.android.mirzalibrary;
using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using TouchType = com.nttqonoq.devices.android.mirzalibrary.TouchType;

public class MiRZAManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI batteryLevelChangedObj;

    [SerializeField]
    private TextMeshProUGUI batteryLevelObj;

    [SerializeField]
    private TextMeshProUGUI chargeStatusObj;

    [SerializeField]
    private DialogPool dialogPool;

    [SerializeField]
    private TextMeshProUGUI displayStatusChangedObj;

    [SerializeField]
    private TextMeshProUGUI glassStatusChangedObj;

    [SerializeField]
    private TextMeshProUGUI glassStatusObj;

    [SerializeField]
    private TextMeshProUGUI glassTouchGestureStatusChangedObj;

    [SerializeField]
    private TextMeshProUGUI pluginVersionObj;

    [SerializeField]
    private TextMeshProUGUI powerOffChangedObj;

    [SerializeField]
    private TextMeshProUGUI serviceStateChangedObj;

    [SerializeField]
    private TextMeshProUGUI spacesModeStatusChangedObj;

    [SerializeField]
    private TextMeshProUGUI spacesModeStatusObj;

    private bool enableOnBatteryLevelChanged = true;
    private bool enableOnDisplayStatusChanged = true;
    private bool enableOnGlassStatusChanged = true;
    private bool enableOnGlassTouchGestureStatusChanged = true;
    private bool enableOnPowerOffChanged = true;
    private bool enableOnServiceStateChanged = true;
    private bool enableOnSpacesModeStatusChanged = true;


    private SynchronizationContext mainThreadContext;
    private MirzaPlugin mirzaPlugin;
    private StringBuilder sb1 = new();
    private StringBuilder sb2 = new();

    public void SetOnBatteryLevelChanged(bool value)
    {
        enableOnBatteryLevelChanged = value;
    }

    public void SetOnDisplayStatusChanged(bool value)
    {
        enableOnDisplayStatusChanged = value;
    }

    public void SetOnGlassStatusChanged(bool value)
    {
        enableOnGlassStatusChanged = value;
    }

    public void SetOnGlassTouchGestureStatusChanged(bool value)
    {
        enableOnGlassTouchGestureStatusChanged = value;
    }

    public void SetOnPowerOffChanged(bool value)
    {
        enableOnPowerOffChanged = value;
    }

    public void SetOnServiceStateChanged(bool value)
    {
        enableOnServiceStateChanged = value;
    }

    public void LaunchMiRZAApp()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        var result = mirzaPlugin?.TransitionToMirzaAppInSpacesMode().Data;

        if (result != null && result == SpacesModeStatus.Off)
#else
        if(true)
#endif
        {
            var dialog = dialogPool.Get();
            dialog.SetHeader("Error");
            dialog.SetBody("Failed to launch MiRZA App.MiRZA is not Spaces Mode.");
            dialog.SetPositive("close");
            dialog.ShowAsync();
        }
    }

    public void SetOnSpacesModeStatusChanged(bool value)
    {
        enableOnSpacesModeStatusChanged = value;
    }

    // Start is called before the first frame update
    private void Start()
    {
        mainThreadContext = SynchronizationContext.Current;

#if UNITY_ANDROID && !UNITY_EDITOR
            // ƒ‰ƒCƒuƒ‰ƒŠ‰Šú‰»
            mirzaPlugin = new MirzaPlugin();
#endif
        mirzaPlugin?.SetLogEnable(true);
        pluginVersionObj.text = mirzaPlugin?.GetVersion();
    }

    public void OnMonitoringStart()
    {
        if (mirzaPlugin == null)
        {
            return;
        }

        if (enableOnBatteryLevelChanged)
            mirzaPlugin.OnBatteryLevelChanged += OnBatteryLevelChanged;
        if (enableOnDisplayStatusChanged)
            mirzaPlugin.OnDisplayStatusChanged += OnDisplayStatusChanged;
        if (enableOnGlassStatusChanged)
            mirzaPlugin.OnGlassStatusChanged += OnGlassStatusChanged;
        if (enableOnGlassTouchGestureStatusChanged)
            mirzaPlugin.OnGlassTouchGestureStatusChanged += OnGlassTouchGestureStatusChanged;
        if (enableOnPowerOffChanged)
            mirzaPlugin.OnPowerOffChanged += OnPowerOffChanged;
        if (enableOnServiceStateChanged)
            mirzaPlugin.OnServiceStateChanged += OnServiceStateChanged;
        if (enableOnSpacesModeStatusChanged)
            mirzaPlugin.OnSpacesModeStatusChanged += OnSpacesModeStatusChanged;

        batteryLevelChangedObj.text = ".....";
        displayStatusChangedObj.text = ".....";
        glassStatusChangedObj.text = ".....";
        glassTouchGestureStatusChangedObj.text = ".....";
        powerOffChangedObj.text = ".....";
        serviceStateChangedObj.text = ".....";
        spacesModeStatusChangedObj.text = ".....";

        mirzaPlugin.StartMonitoring();
    }

    public void OnMonitoringStop()
    {
        if (mirzaPlugin == null)
        {
            return;
        }

        mirzaPlugin.StopMonitoring();

        if (enableOnBatteryLevelChanged)
            mirzaPlugin.OnBatteryLevelChanged -= OnBatteryLevelChanged;
        if (enableOnDisplayStatusChanged)
            mirzaPlugin.OnDisplayStatusChanged -= OnDisplayStatusChanged;
        if (enableOnGlassStatusChanged)
            mirzaPlugin.OnGlassStatusChanged -= OnGlassStatusChanged;
        if (enableOnGlassTouchGestureStatusChanged)
            mirzaPlugin.OnGlassTouchGestureStatusChanged -= OnGlassTouchGestureStatusChanged;
        if (enableOnPowerOffChanged)
            mirzaPlugin.OnPowerOffChanged -= OnPowerOffChanged;
        if (enableOnServiceStateChanged)
            mirzaPlugin.OnServiceStateChanged -= OnServiceStateChanged;
        if (enableOnSpacesModeStatusChanged)
            mirzaPlugin.OnSpacesModeStatusChanged -= OnSpacesModeStatusChanged;
    }

    private void OnSpacesModeStatusChanged(SpacesModeStatus obj)
    {
        PostMessage(spacesModeStatusChangedObj, Enum.GetName(typeof(SpacesModeStatus), obj));
    }

    private void OnServiceStateChanged(ServiceState obj)
    {
        PostMessage(serviceStateChangedObj, Enum.GetName(typeof(ServiceState), obj));
    }

    private void OnPowerOffChanged()
    {
        PostMessage(powerOffChangedObj, "Off");
    }

    private void OnGlassTouchGestureStatusChanged(GlassTouchGestureStatus obj)
    {
        sb1.Clear();
        sb1.AppendLine($"Movement -> {obj.Movement}");
        sb1.AppendLine($"Operation -> {Enum.GetName(typeof(TouchOperation), obj.Operation)}");
        sb1.AppendLine($"Type -> {Enum.GetName(typeof(TouchType), obj.Type)}");
        sb1.AppendLine($"XCoordinate -> {obj.XCoordinate}");
        sb1.AppendLine($"YCoordinate -> {obj.YCoordinate}");
        PostMessage(glassTouchGestureStatusChangedObj, sb1.ToString());
    }

    private void OnGlassStatusChanged(GlassStatus obj)
    {
        sb2.Clear();
        sb2.AppendLine($"BluetoothStatus -> {Enum.GetName(typeof(ConnectionStatus), obj.BluetoothStatus)}");
        sb2.AppendLine($"SpacesAvailable -> {obj.SpacesAvailable}");
        sb2.AppendLine($"WifiStatus -> {Enum.GetName(typeof(ConnectionStatus), obj.WifiStatus)}");
        PostMessage(glassStatusChangedObj, sb2.ToString());
    }

    private void OnDisplayStatusChanged(DisplayStatus obj)
    {
        PostMessage(displayStatusChangedObj, $"{Enum.GetName(typeof(DisplayStatus), obj)}");
    }

    private void OnBatteryLevelChanged(int obj)
    {
        PostMessage(batteryLevelChangedObj, obj.ToString());
    }

    public async void GetMiRZAStatus()
    {
        mirzaPlugin.GetBatteryLevelAsync(x => PostMessage(batteryLevelObj, x.Data.ToString()));
        mirzaPlugin.GetChargeStatusAsync(x => PostMessage(chargeStatusObj, Enum.GetName(typeof(ChargeStatus), x.Data)));
        mirzaPlugin.GetGlassStatusAsync(x => PostMessage(glassStatusObj,
            $"BluetoothStatus -> {Enum.GetName(typeof(ConnectionStatus), x.Data.BluetoothStatus)}\nSpacesAvailable -> {x.Data.SpacesAvailable}\nWifiStatus -> {Enum.GetName(typeof(ConnectionStatus), x.Data.WifiStatus)}"));
        mirzaPlugin.GetSpacesModeStatusAsync(x =>
            PostMessage(spacesModeStatusObj, Enum.GetName(typeof(SpacesModeStatus), x.Data)));
    }

    private void PostMessage(TextMeshProUGUI targetText, string text)
    {
        mainThreadContext.Post(_ => { targetText.text = text; }, null);
    }
}