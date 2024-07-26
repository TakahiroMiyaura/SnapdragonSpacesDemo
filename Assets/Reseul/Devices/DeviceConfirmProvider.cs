// Copyright (c) 2023 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;

namespace Reseul.Snapdragon.Spaces.Devices
{

    public enum XRDeviceType
    {
        Unknown,
        ThinkRealityA3,
        ThinkRealityVRX
    }

    public class DeviceConfirmProvider
    {

        public static XRDeviceType GetCurrentDeviceType()
        {

            var modelName = SystemInfo.deviceModel.ToLower();

            if (modelName.Contains("vrx"))
            {
                return XRDeviceType.ThinkRealityVRX;
            }
            else if (modelName.Contains("motorola"))
            {
                //TODO: ThinkRealityA3のモデル名がわからないので、とりあえずMotorolaの文字列が入っていたらThinkRealityA3として扱う
                return XRDeviceType.ThinkRealityA3;
            }
            else
            {
                return XRDeviceType.Unknown;
            }
        }
    }
}