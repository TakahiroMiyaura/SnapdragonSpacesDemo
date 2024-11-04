// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using MixedReality.Toolkit.SpatialManipulation;
using UnityEngine;

namespace Reseul.Snapdragon.Spaces.Utilities
{
    public class DialogModeSwitcher : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private SolverHandler solverHandler;

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            var type = DeviceConfirmProvider.GetCurrentDeviceType();

            switch (type)
            {
                case XRDeviceType.ThinkRealityVRX:
                    canvas.renderMode = RenderMode.WorldSpace;
                    if (gameObject.GetComponent<Follow>() == null)
                    {
                        var followSolver = gameObject.AddComponent<Follow>();
                        followSolver.Smoothing = true;
                        followSolver.MoveLerpTime = 1.0f;
                        followSolver.RotateLerpTime = 1.0f;
                        followSolver.OrientToControllerDeadZoneDegrees = 25.0f;
                    }

                    solverHandler.UpdateSolvers = true;
                    break;
                case XRDeviceType.Handheld:
                    solverHandler.UpdateSolvers = false;
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    break;
                case XRDeviceType.Unknown:
                    break;
            }
        }
    }
}