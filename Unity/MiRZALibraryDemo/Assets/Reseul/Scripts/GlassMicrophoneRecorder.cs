// Copyright (c) 2025 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using System.IO;
using Qualcomm.Snapdragon.Spaces;
using UnityEngine;
using UnityEngine.Android;

public class GlassMicrophoneRecorder : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 50f)]
    private float amplifire = 1f;

    [SerializeField]
    private AudioSource audioSource;

    private AndroidJavaObject audioRecorderHandler;
    private string filePath;
    private bool isRecording;

    private void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            var nativeXRSupportChecker =
                new AndroidJavaClass("com.qualcomm.snapdragon.spaces.serviceshelper.NativeXRSupportChecker");
            if (nativeXRSupportChecker.CallStatic<bool>("CanShowPermissions") ||
                FeatureUseCheckUtility.IsFeatureEnabled(FeatureUseCheckUtility.SpacesFeature.Fusion))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
        }
    }

    private void InitializeJavaHandler()
    {
        audioRecorderHandler = new AndroidJavaObject("com.nttqonoq.mirza.recordlibrary.AudioRecorderHandler");
    }

    // ˜^‰¹‚ÌŠJŽn
    public void StartRecording()
    {
        if (isRecording)
        {
            return;
        }

        InitializeJavaHandler();

        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var context = activity.Call<AndroidJavaObject>("getApplicationContext");
            var audioManager = context.Call<AndroidJavaObject>("getSystemService", "audio");

            // AudioDeviceInfo‚Ì”z—ñ‚ðŽæ“¾
            var devices = audioManager.Call<AndroidJavaObject[]>("getDevices", 1); // GET_DEVICES_INPUTS

            AndroidJavaObject proxyDevice = null;

            foreach (var device in devices)
            {
                var deviceType = device.Call<int>("getType");
                if (deviceType == 20) // TYPE_IP
                {
                    proxyDevice = device;
                    break;
                }
            }

            if (proxyDevice != null)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                var dateTime = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                var fileName = $"audio_recording_{dateTime}.pcm";
                filePath = $"{Application.persistentDataPath}/{fileName}";

                audioRecorderHandler.Call("startRecordingWithDevice", proxyDevice, filePath);

                isRecording = true;
            }
            else
            {
                Debug.LogError("in-proxy device‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
            }
        }
    }

    // ˜^‰¹‚Ì’âŽ~
    public void StopRecording()
    {
        if (!isRecording)
        {
            return;
        }

        isRecording = false;

        if (audioRecorderHandler != null)
        {
            audioRecorderHandler.Call("stopRecording");
            audioRecorderHandler.Dispose();
        }
    }

    public void SelectChanged(int index)
    {
        switch (index)
        {
            case 0:
                StartRecording();
                break;
            case 1:
                StopRecording();
                StopPlayOneShot();
                break;
            case 2:
                PlayOneShot();
                break;
        }
    }


    private void StopPlayOneShot()
    {
        if (!audioSource.isPlaying)
        {
            return;
        }

        audioSource.Stop();
    }

    private void PlayOneShot()
    {
        if (isRecording)
        {
            return;
        }

        using var fileStream = new FileStream(filePath, FileMode.Open);
        var audioClipData = new byte[fileStream.Length];
        fileStream.Read(audioClipData, 0, audioClipData.Length);
        var audioClip = AudioClip.Create("MiRZA Audio", audioClipData.Length, 1, 44100, false);
        audioClip.SetData(Create16BITAudioClipData(audioClipData, amplifire), 0);
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private static float[] Create16BITAudioClipData(byte[] data, float amp)
    {
        var audioClipData = new float[data.Length / 2];
        var memoryStream = new MemoryStream(data);

        for (var i = 0;; i++)
        {
            var target = new byte[2];
            var read = memoryStream.Read(target);

            if (read <= 0) break;

            audioClipData[i] = (float)BitConverter.ToInt16(target) / short.MaxValue * amp;
        }

        return audioClipData;
    }
}