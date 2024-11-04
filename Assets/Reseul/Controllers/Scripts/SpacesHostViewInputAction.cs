// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class SpacesHostViewInputAction : MonoBehaviour
    {
        private SpacesHostViewDeviceState deviceState;
        private SpacesHostViewDevice spacesHostViewDevice;

        private void OnEnable()
        {
            spacesHostViewDevice = InputSystem.GetDevice<SpacesHostViewDevice>();
            deviceState = new SpacesHostViewDeviceState();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            deviceState.MobileDisplayPosition = transform.position;
            deviceState.MobileDisplayRotation = transform.rotation;
            InputSystem.QueueStateEvent(spacesHostViewDevice, deviceState, Time.realtimeSinceStartup);
        }
    }
}