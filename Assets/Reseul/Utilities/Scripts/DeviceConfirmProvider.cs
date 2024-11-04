// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Qualcomm.Snapdragon.Spaces;
using UnityEngine;
using UnityEngine.XR.OpenXR;

namespace Reseul.Snapdragon.Spaces.Utilities
{
    public enum XRDeviceType
    {
        Unknown,
        Handheld,
        ThinkRealityVRX,
        Console,
        Desktop
    }

    public class DeviceConfirmProvider
    {
        public static XRDeviceType GetCurrentDeviceType()
        {
            var baseRuntimeFeature = OpenXRSettings.Instance.GetFeature<BaseRuntimeFeature>();
            baseRuntimeFeature.IsFusionSupported();

            var modelName = SystemInfo.graphicsDeviceName;

            if (modelName.Contains("vrx"))
            {
                return XRDeviceType.ThinkRealityVRX;
            }

            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                return XRDeviceType.Handheld;
            }

            return XRDeviceType.Unknown;
        }
    }
}