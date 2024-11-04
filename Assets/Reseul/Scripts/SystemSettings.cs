// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using MixedReality.Toolkit.UX;
using Qualcomm.Snapdragon.Spaces;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Reseul.Snapdragon.Spaces.Utilities
{
    public class SystemSettings : MonoBehaviour
    {
        private static SystemSettings instance;

        [SerializeField]
        private PressableButton AutoManageXRCameraButton;

        [SerializeField]
        private PressableButton AutoStartOnDisplayConnectedButton;

        [SerializeField]
        private PressableButton DisplayDebugControllerButton;

        [SerializeField]
        private PressableButton DisplayDebugHostViewButton;

        [SerializeField]
        private PressableButton DisplayDebugOpenXRRButton;

        [SerializeField]
        private PressableButton DisplayDebugTouchScreenButton;

        [SerializeField]
        private PressableButton LeftHandTrackingButton;

        [SerializeField]
        private DynamicOpenXRLoader loader;

        [SerializeField]
        private PressableButton RightHandTrackingButton;

        public static SystemSettings Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType<SystemSettings>();
                return instance;
            }
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

        public bool LeftHandTracking
        {
            get
            {
                if (PlayerPrefs.HasKey("LeftHandTracking"))
                {
                    return PlayerPrefs.GetInt("LeftHandTracking") == 1;
                }

                PlayerPrefs.SetInt("LeftHandTracking", 0);
                return false;
            }
            set => PlayerPrefs.SetInt("LeftHandTracking", value ? 1 : 0);
        }

        public bool RightHandTracking
        {
            get
            {
                if (PlayerPrefs.HasKey("RightHandTracking"))
                {
                    return PlayerPrefs.GetInt("RightHandTracking") == 1;
                }

                PlayerPrefs.SetInt("RightHandTracking", 0);
                return false;
            }
            set => PlayerPrefs.SetInt("RightHandTracking", value ? 1 : 0);
        }

        private void Awake()
        {
            loader.AutoStartXROnDisplayConnected = AutoStartOnDisplayConnected;
            loader.AutoManageXRCamera = AutoManageXRCamera;
        }

        private void Start()
        {
            AutoStartOnDisplayConnectedButton?.ForceSetToggled(AutoStartOnDisplayConnected);
            AutoManageXRCameraButton?.ForceSetToggled(AutoManageXRCamera);
            DisplayDebugOpenXRRButton?.ForceSetToggled(DisplayDebugOpenXR);
            DisplayDebugHostViewButton?.ForceSetToggled(DisplayDebugHostView);
            DisplayDebugControllerButton?.ForceSetToggled(DisplayDebugController);
            DisplayDebugTouchScreenButton?.ForceSetToggled(DisplayDebugTouchScreen);
            LeftHandTrackingButton?.ForceSetToggled(LeftHandTracking);
            RightHandTrackingButton?.ForceSetToggled(RightHandTracking);
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(SystemSettings))]
    internal class SystemSettingsEditor : Editor
    {
        private SerializedProperty autoManageXRCameraButton;
        private SerializedProperty autoStartOnDisplayConnectedButton;
        private SerializedProperty displayDebugControllerButton;
        private SerializedProperty displayDebugHostViewButton;
        private SerializedProperty displayDebugOpenXRRButton;
        private SerializedProperty displayDebugTouchScreenButton;
        private SerializedProperty leftHandTrackingButton;
        private SerializedProperty loader;
        private SerializedProperty rightHandTrackingButton;
        private bool showDebugSettings = true;
        private bool showHostViewSettings = true;
        private bool showLoaderSettings = true;

        public void OnEnable()
        {
            autoManageXRCameraButton = serializedObject.FindProperty("AutoManageXRCameraButton");
            autoStartOnDisplayConnectedButton = serializedObject.FindProperty("AutoStartOnDisplayConnectedButton");
            displayDebugControllerButton = serializedObject.FindProperty("DisplayDebugControllerButton");
            displayDebugHostViewButton = serializedObject.FindProperty("DisplayDebugHostViewButton");
            displayDebugOpenXRRButton = serializedObject.FindProperty("DisplayDebugOpenXRRButton");
            displayDebugTouchScreenButton = serializedObject.FindProperty("DisplayDebugTouchScreenButton");
            leftHandTrackingButton = serializedObject.FindProperty("LeftHandTrackingButton");
            rightHandTrackingButton = serializedObject.FindProperty("RightHandTrackingButton");
            loader = serializedObject.FindProperty("loader");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            showLoaderSettings = EditorGUILayout.Foldout(showLoaderSettings, "Dynamic OpenXR Loader Settings");
            if (showLoaderSettings)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(loader);
                EditorGUILayout.PropertyField(autoManageXRCameraButton);
                EditorGUILayout.PropertyField(autoStartOnDisplayConnectedButton);
                EditorGUI.indentLevel--;
            }

            showDebugSettings = EditorGUILayout.Foldout(showDebugSettings, "Display Debug Info Settings");
            if (showDebugSettings)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(displayDebugControllerButton);
                EditorGUILayout.PropertyField(displayDebugHostViewButton);
                EditorGUILayout.PropertyField(displayDebugOpenXRRButton);
                EditorGUILayout.PropertyField(displayDebugTouchScreenButton);
                EditorGUI.indentLevel--;
            }

            showHostViewSettings = EditorGUILayout.Foldout(showHostViewSettings, "Host View Info Settings");
            if (showHostViewSettings)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(leftHandTrackingButton);
                EditorGUILayout.PropertyField(rightHandTrackingButton);
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}