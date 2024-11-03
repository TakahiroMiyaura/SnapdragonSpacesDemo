// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using MixedReality.Toolkit.UX;
using UnityEngine;

namespace Reseul.Snapdragon.Spaces.Samples.DualRenderFusionMRTK3
{
    public class SystemSettings : MonoBehaviour
    {
        private static SystemSettings instance;

        public static SystemSettings Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType<SystemSettings>();
                return instance;
            }
        }

        [SerializeField]
        private PressableButton AutoStartOnDisplayConnectedButton;
        [SerializeField]
        private PressableButton AutoManageXRCameraButton;
        [SerializeField]
        private PressableButton DisplayDebugOpenXRRButton;
        [SerializeField]
        private PressableButton DisplayDebugHostViewButton;
        [SerializeField]
        private PressableButton DisplayDebugControllerButton;
        [SerializeField]
        private PressableButton DisplayDebugTouchScreenButton;

        void Start()
        {
            AutoStartOnDisplayConnectedButton?.ForceSetToggled(AutoStartOnDisplayConnected);
            AutoManageXRCameraButton?.ForceSetToggled(AutoManageXRCamera);
            DisplayDebugOpenXRRButton?.ForceSetToggled(DisplayDebugOpenXR);
            DisplayDebugHostViewButton?.ForceSetToggled(DisplayDebugHostView);
            DisplayDebugControllerButton?.ForceSetToggled(DisplayDebugController);
            DisplayDebugTouchScreenButton?.ForceSetToggled(DisplayDebugTouchScreen);
        }
        public bool AutoStartOnDisplayConnected
        {
            get
            {
                if (PlayerPrefs.HasKey("AutoStartOnDisplayConnected"))
                {
                    return PlayerPrefs.GetInt("AutoStartOnDisplayConnected") == 1;
                }

                PlayerPrefs.SetInt("AutoStartOnDisplayConnected", 0);
                return false;
            }
            set => PlayerPrefs.SetInt("AutoStartOnDisplayConnected", value ? 1 : 0);
        }

        public bool AutoManageXRCamera
        {
            get
            {
                if (PlayerPrefs.HasKey("AutoManageXRCamera"))
                {
                    return PlayerPrefs.GetInt("AutoManageXRCamera") == 1;
                }

                PlayerPrefs.SetInt("AutoManageXRCamera", 1);
                return true;
            }
            set => PlayerPrefs.SetInt("AutoManageXRCamera", value ? 1 : 0);
        }

        public bool DisplayDebugOpenXR
        {
            get
            {
                if (PlayerPrefs.HasKey("DisplayDebugOpenXR"))
                {
                    return PlayerPrefs.GetInt("DisplayDebugOpenXR") == 1;
                }

                PlayerPrefs.SetInt("DisplayDebugOpenXR", 1);
                return true;
            }
            set => PlayerPrefs.SetInt("DisplayDebugOpenXR", value ? 1 : 0);
        }

        public bool DisplayDebugHostView
        {
            get
            {
                if (PlayerPrefs.HasKey("DisplayDebugHostView"))
                {
                    return PlayerPrefs.GetInt("DisplayDebugHostView") == 1;
                }

                PlayerPrefs.SetInt("DisplayDebugHostView", 1);
                return true;
            }
            set => PlayerPrefs.SetInt("DisplayDebugHostView", value ? 1 : 0);
        }

        public bool DisplayDebugController
        {
            get
            {
                if (PlayerPrefs.HasKey("DisplayDebugController"))
                {
                    return PlayerPrefs.GetInt("DisplayDebugController") == 1;
                }

                PlayerPrefs.SetInt("DisplayDebugController", 1);
                return true;
            }
            set => PlayerPrefs.SetInt("DisplayDebugController", value ? 1 : 0);
        }

        public bool DisplayDebugTouchScreen
        {
            get
            {
                if (PlayerPrefs.HasKey("DisplayDebugTouchScreen"))
                {
                    return PlayerPrefs.GetInt("DisplayDebugTouchScreen") == 1;
                }

                PlayerPrefs.SetInt("DisplayDebugTouchScreen", 1);
                return true;
            }
            set => PlayerPrefs.SetInt("DisplayDebugTouchScreen", value ? 1 : 0);
        }
    }
}