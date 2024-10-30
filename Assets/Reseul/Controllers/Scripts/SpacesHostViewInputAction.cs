// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class SpacesHostViewInputAction : MonoBehaviour
    {
        private SpacesHostViewDevice spacesHostViewDevice;
        private SpacesHostViewDeviceState spacesHostViewDeviceState1;

        private void OnEnable()
        {
            spacesHostViewDevice = InputSystem.GetDevice<SpacesHostViewDevice>();
            spacesHostViewDeviceState1 = new SpacesHostViewDeviceState();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            spacesHostViewDeviceState1.MobileDisplayPosition = transform.position;
            spacesHostViewDeviceState1.MobileDisplayRotation = transform.rotation;
            InputSystem.QueueStateEvent(spacesHostViewDevice, spacesHostViewDeviceState1, Time.realtimeSinceStartup);
        }
    }
}