// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Qualcomm.Snapdragon.Spaces;
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
        private InputActionReference hostViewRotation;

        [SerializeField]
        private TextMeshProUGUI hostViewPositionText = null;

        [SerializeField]
        private TextMeshProUGUI hostViewRotationText = null;
        private void OnEnable()
        {
            hostViewPosition.action.performed += ctx =>
                hostViewPositionText.text =
                    $"({ctx.ReadValue<Vector3>().x:F2},{ctx.ReadValue<Vector3>().y:F2},{ctx.ReadValue<Vector3>().z:F2})";
            hostViewRotation.action.performed += ctx =>
            {
                var rotation = ctx.ReadValue<Quaternion>();
                rotation.ToAngleAxis(out var angle, out var axis);
                hostViewRotationText.text =
                    $"{angle:F1}°({axis.x:F2},{axis.y:F2},{axis.z:F2})";
            };
        }

    }
}