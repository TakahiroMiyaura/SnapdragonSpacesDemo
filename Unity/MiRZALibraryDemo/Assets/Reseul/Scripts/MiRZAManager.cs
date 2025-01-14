// Copyright (c) 2025 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using com.nttqonoq.devices.android.mirzalibrary;
using MixedReality.Toolkit.UX;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using TouchType = com.nttqonoq.devices.android.mirzalibrary.TouchType;

public class GlassTouchGestureStatusChangedEvent : UnityEvent<GlassTouchGestureStatus>
{
}

public class MiRZAManager : MonoBehaviour
{
    private static object lockObject = new();
    private static MiRZAManager selfInstance;

    [SerializeField]
    private ToggleCollection audioButtons;

    [SerializeField]
    private AudioSource audioSource;

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

    private string deviceName;

    private bool enableOnBatteryLevelChanged = true;
    private bool enableOnDisplayStatusChanged = true;
    private bool enableOnGlassStatusChanged = true;
    private bool enableOnGlassTouchGestureStatusChanged = true;
    private bool enableOnPowerOffChanged = true;
    private bool enableOnServiceStateChanged = true;
    private bool enableOnSpacesModeStatusChanged = true;
    private MicMode frontMicMode;


    private SynchronizationContext mainThreadContext;
    private MirzaPlugin mirzaPlugin;
    private MixMode mixMode;

    public GlassTouchGestureStatusChangedEvent OnGlassTouchGestureStatusChanged = new GlassTouchGestureStatusChangedEvent();
    private StringBuilder sb1 = new();
    private StringBuilder sb2 = new();

    private MicMode wearingMicMode;

    public static MiRZAManager Instance
    {
        get
        {
            lock (lockObject)
            {
                if (selfInstance == null)
                {
                    selfInstance = FindObjectOfType<MiRZAManager>();
                }
            }

            return selfInstance;
        }
    }

    public GlassTouchGestureStatus GestureStatus { get; private set; }

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
        if (true)
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
            // ライブラリ初期化
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
            mirzaPlugin.OnBatteryLevelChanged += OnBatteryLevelChangedInternalAction;
        if (enableOnDisplayStatusChanged)
            mirzaPlugin.OnDisplayStatusChanged += OnDisplayStatusChangedInternalAction;
        if (enableOnGlassStatusChanged)
            mirzaPlugin.OnGlassStatusChanged += OnGlassStatusChangedInternalAction;
        if (enableOnGlassTouchGestureStatusChanged)
            mirzaPlugin.OnGlassTouchGestureStatusChanged += OnGlassTouchGestureStatusChangedInternalAction;
        if (enableOnPowerOffChanged)
            mirzaPlugin.OnPowerOffChanged += OnPowerOffChangedInternalAction;
        if (enableOnServiceStateChanged)
            mirzaPlugin.OnServiceStateChanged += OnServiceStateChangedInternalAction;
        if (enableOnSpacesModeStatusChanged)
            mirzaPlugin.OnSpacesModeStatusChanged += OnSpacesModeStatusChangedInternalAction;

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
            mirzaPlugin.OnBatteryLevelChanged -= OnBatteryLevelChangedInternalAction;
        if (enableOnDisplayStatusChanged)
            mirzaPlugin.OnDisplayStatusChanged -= OnDisplayStatusChangedInternalAction;
        if (enableOnGlassStatusChanged)
            mirzaPlugin.OnGlassStatusChanged -= OnGlassStatusChangedInternalAction;
        if (enableOnGlassTouchGestureStatusChanged)
            mirzaPlugin.OnGlassTouchGestureStatusChanged -= OnGlassTouchGestureStatusChangedInternalAction;
        if (enableOnPowerOffChanged)
            mirzaPlugin.OnPowerOffChanged -= OnPowerOffChangedInternalAction;
        if (enableOnServiceStateChanged)
            mirzaPlugin.OnServiceStateChanged -= OnServiceStateChangedInternalAction;
        if (enableOnSpacesModeStatusChanged)
            mirzaPlugin.OnSpacesModeStatusChanged -= OnSpacesModeStatusChangedInternalAction;
    }

    private void OnSpacesModeStatusChangedInternalAction(SpacesModeStatus obj)
    {
        PostMessage(spacesModeStatusChangedObj, Enum.GetName(typeof(SpacesModeStatus), obj));
    }

    private void OnServiceStateChangedInternalAction(ServiceState obj)
    {
        PostMessage(serviceStateChangedObj, Enum.GetName(typeof(ServiceState), obj));
    }

    private void OnPowerOffChangedInternalAction()
    {
        PostMessage(powerOffChangedObj, "Off");
    }

    public void SpacesModeOn()
    {
        mirzaPlugin?.SpacesModeOnAsync(x =>
        {
            mainThreadContext.Post(_ =>
            {
                var dialog = dialogPool.Get();
                dialog.SetHeader("Spaces Mode");
                dialog.SetBody(
                    $"Switch Spaces Mode - ON.\nExit Code{x.Data}");
                dialog.SetPositive("close");
                dialog.ShowAsync();
            }, null);
        });
    }

    public void SpacesModeOff()
    {
        mirzaPlugin?.SpacesModeOffAsync(x =>
        {
            mainThreadContext.Post(_ =>
            {
                var dialog = dialogPool.Get();
                dialog.SetHeader("Spaces Mode");
                dialog.SetBody(
                    $"Switch Spaces Mode - OFF.\nExit Code{x.Data}");
                dialog.SetPositive("close");
                dialog.ShowAsync();
            }, null);
        });
    }

    public void SetSpacesModeStatus(TextMeshProUGUI text)
    {
        mirzaPlugin?.GetSpacesModeStatusAsync(x =>
            mainThreadContext.Post(_ => { text.text = Enum.GetName(typeof(SpacesModeStatus), x.Data); }, null));
    }

    private void OnGlassTouchGestureStatusChangedInternalAction(GlassTouchGestureStatus obj)
    {
        OnGlassTouchGestureStatusChanged.Invoke(obj);
        GestureStatus = obj;
        var movement = obj.Movement;
        var touchOperation = Enum.GetName(typeof(TouchOperation), obj.Operation);
        var touchType = Enum.GetName(typeof(TouchType), obj.Type);
        var xCoordinate = obj.XCoordinate;
        var yCoordinate = obj.YCoordinate;
        sb1.Clear();
        sb1.AppendLine($"Movement -> {movement}");
        sb1.AppendLine($"Operation -> {touchOperation}");
        sb1.AppendLine($"Type -> {touchType}");
        sb1.AppendLine($"XCoordinate -> {xCoordinate}");
        sb1.AppendLine($"YCoordinate -> {yCoordinate}");
        PostMessage(glassTouchGestureStatusChangedObj, sb1.ToString());
    }

    private void OnGlassStatusChangedInternalAction(GlassStatus obj)
    {
        sb2.Clear();
        sb2.AppendLine($"BluetoothStatus -> {Enum.GetName(typeof(ConnectionStatus), obj.BluetoothStatus)}");
        sb2.AppendLine($"SpacesAvailable -> {obj.SpacesAvailable}");
        sb2.AppendLine($"WifiStatus -> {Enum.GetName(typeof(ConnectionStatus), obj.WifiStatus)}");
        PostMessage(glassStatusChangedObj, sb2.ToString());
    }

    private void OnDisplayStatusChangedInternalAction(DisplayStatus obj)
    {
        PostMessage(displayStatusChangedObj, $"{Enum.GetName(typeof(DisplayStatus), obj)}");
    }

    private void OnBatteryLevelChangedInternalAction(int obj)
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

    public void OnMicMode1(int index)
    {
        wearingMicMode = (MicMode)index;
        OnMicMode2(index);
    }

    public void OnMicMode2(int index)
    {
        frontMicMode = (MicMode)index;
    }


    public void StopRecOrPlay()
    {
        if (Microphone.IsRecording(deviceName))
            Microphone.End(deviceName);
        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    public void PlayOneShot()
    {
        StopRecOrPlay();
        audioSource.PlayOneShot(audioSource.clip);
        Task.Delay(new TimeSpan(0, 0, 0, 31)).ContinueWith(_ =>
        {
            mainThreadContext.Post(__ => { audioButtons.CurrentIndex = 1; }, null);
        });
    }

    public void SetMixMode(TextMeshProUGUI text)
    {
        //常にOffが現在の仕様
        mixMode = wearingMicMode == frontMicMode ? MixMode.Off: MixMode.On ;

        text.text = Enum.GetName(typeof(MixMode), mixMode);

        mirzaPlugin?.SwitchMicrophoneAsync(wearingMicMode, frontMicMode, mixMode, x =>
        {
            mainThreadContext.Post(_ =>
            {
                var dialog = dialogPool.Get();
                dialog.SetHeader("Information");
                dialog.SetBody(
                    $"Switch Microphone Mode.\n WearMicMode:{x.Data.WearingMicMode}\n FrontMicMode:{x.Data.FrontMicMode}\n MixMode:{x.Data.MixMode}");
                dialog.SetPositive("close");
                dialog.ShowAsync();
            }, null);
        });
    }
}