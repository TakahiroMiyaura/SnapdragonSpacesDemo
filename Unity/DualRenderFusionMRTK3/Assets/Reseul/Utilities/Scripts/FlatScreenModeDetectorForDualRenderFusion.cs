// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System.Collections.Generic;
using MixedReality.Toolkit;
using MixedReality.Toolkit.Input;
using UnityEngine;

namespace Reseul.Snapdragon.Spaces.Utilities
{
    internal class FlatScreenModeDetectorForDualRenderFusion : MonoBehaviour, IInteractionModeDetector
    {
        [SerializeField]
        private List<GameObject> controllers;

        [SerializeField]
        private InteractionMode flatScreenInteractionMode;

        [SerializeField]
        private bool forceModeDetected = false;

        protected ControllerLookup controllerLookup;


        public InteractionMode ModeOnDetection => flatScreenInteractionMode;

        /// <inheritdoc />
        public List<GameObject> GetControllers()
        {
            return controllers;
        }

        public bool IsModeDetected()
        {
            return forceModeDetected ||
                   (!controllerLookup.LeftHandController.currentControllerState.inputTrackingState
                       .HasPositionAndRotation() && !controllerLookup.RightHandController.currentControllerState
                       .inputTrackingState.HasPositionAndRotation());
        }

        protected void Awake()
        {
            controllerLookup = ComponentCache<ControllerLookup>.FindFirstActiveInstance();
        }
    }
}