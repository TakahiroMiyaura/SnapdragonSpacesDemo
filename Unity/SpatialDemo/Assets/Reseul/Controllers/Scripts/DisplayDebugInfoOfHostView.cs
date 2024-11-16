// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class DisplayDebugInfoOfHostView : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference hostViewPosition;

        [SerializeField]
        private TextMeshProUGUI hostViewPositionText = null;

        [SerializeField]
        private InputActionReference hostViewRotation;

        [SerializeField]
        private TextMeshProUGUI hostViewRotationText = null;

        private void OnEnable()
        {
            hostViewPosition.action.performed += HostViewPositionPerformed;
            hostViewRotation.action.performed += HostViewRotationPerformed;
        }

        private void OnDisable()
        {
            hostViewPosition.action.performed -= HostViewPositionPerformed;
            hostViewRotation.action.performed -= HostViewRotationPerformed;
        }

        private void HostViewPositionPerformed(InputAction.CallbackContext ctx)
        {
            hostViewPositionText.text =
                $"({ctx.ReadValue<Vector3>().x:F2},{ctx.ReadValue<Vector3>().y:F2},{ctx.ReadValue<Vector3>().z:F2})";
        }

        private void HostViewRotationPerformed(InputAction.CallbackContext ctx)
        {
            var rotation = ctx.ReadValue<Quaternion>();
            rotation.ToAngleAxis(out var angle, out var axis);
            hostViewRotationText.text =
                $"{angle:F1}°({axis.x:F2},{axis.y:F2},{axis.z:F2})";
        }
    }
}