// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System.Collections.Generic;
using MixedReality.Toolkit;
using MixedReality.Toolkit.Input;
using UnityEngine;

namespace Assets.Reseul
{
    internal class FlatScreenModeDetectorForDualRenderFusion : MonoBehaviour, IInteractionModeDetector
    {
        protected ControllerLookup controllerLookup;

        [SerializeField]
        private InteractionMode flatScreenInteractionMode;

        [SerializeField]
        private List<GameObject> controllers;

        [SerializeField]
        private bool forceModeDetected = false;


        public InteractionMode ModeOnDetection => flatScreenInteractionMode;

        /// <inheritdoc />
        public List<GameObject> GetControllers()
        {
            return controllers;
        }

        public bool IsModeDetected()
        {
            return forceModeDetected || !controllerLookup.LeftHandController.currentControllerState.inputTrackingState.HasPositionAndRotation() && !controllerLookup.RightHandController.currentControllerState.inputTrackingState.HasPositionAndRotation();
        }

        protected void Awake()
        {
            controllerLookup = ComponentCache<ControllerLookup>.FindFirstActiveInstance();
        }
    }
}