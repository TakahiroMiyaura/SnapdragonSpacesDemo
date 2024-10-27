// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
#if UNITY_EDITOR
using UnityEditor.AnimatedValues;
#endif

namespace Assets.Reseul.MobileStickController.Scripts
{

    public class OnScreenStickDelta : OnScreenStick, IPointerDownHandler, IPointerUpHandler,IDragHandler
    {
        [InputControl(layout = "Delta")]
        [SerializeField]
        private string controlPathDelta;

        private InputControl controlDelta;

        protected override void OnEnable()
        {
            base.OnEnable();
            controlDelta = InputControlPath.TryFindControl(this.control.device, controlPathDelta);

        }
        
        public new void OnPointerDown(PointerEventData eventData)
        {
            
            base.OnPointerDown(eventData);
            InputSystem.QueueDeltaStateEvent(controlDelta,eventData.delta,Time.realtimeSinceStartup);
        }
        
        public new void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            InputSystem.QueueDeltaStateEvent(controlDelta, Vector2.zero, Time.realtimeSinceStartup);
        }

        public new void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            InputSystem.QueueDeltaStateEvent(controlDelta, eventData.delta, Time.realtimeSinceStartup);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(OnScreenStickDelta))]
    internal class OnScreenStickDeltaEditor : Editor
    {
        private SerializedProperty controlPathDelta;
        private AnimBool showDynamicOriginOptions;
        private AnimBool showIsolatedInputActions;

        private SerializedProperty useIsolatedInputActions;
        private SerializedProperty behaviour;
        private SerializedProperty controlPathInternal;
        private SerializedProperty movementRange;
        private SerializedProperty dynamicOriginRange;
        private SerializedProperty pointerDownAction;
        private SerializedProperty pointerMoveAction;

        public void OnEnable()
        {
            showDynamicOriginOptions = new AnimBool(false);
            showIsolatedInputActions = new AnimBool(false);

            useIsolatedInputActions = serializedObject.FindProperty("m_UseIsolatedInputActions");

            behaviour = serializedObject.FindProperty("m_Behaviour");
            controlPathInternal = serializedObject.FindProperty("m_ControlPath");
            movementRange = serializedObject.FindProperty("m_MovementRange");
            dynamicOriginRange = serializedObject.FindProperty("m_DynamicOriginRange");
            pointerDownAction = serializedObject.FindProperty("m_PointerDownAction");
            pointerMoveAction = serializedObject.FindProperty("m_PointerMoveAction");

            controlPathDelta = serializedObject.FindProperty("controlPathDelta");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(movementRange);
            EditorGUILayout.PropertyField(controlPathInternal);
            EditorGUILayout.PropertyField(controlPathDelta);
            EditorGUILayout.PropertyField(behaviour);

            showDynamicOriginOptions.target = ((OnScreenStick)target).behaviour ==
                                                OnScreenStick.Behaviour.ExactPositionWithDynamicOrigin;
            if (EditorGUILayout.BeginFadeGroup(showDynamicOriginOptions.faded))
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(dynamicOriginRange);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();

            EditorGUILayout.PropertyField(useIsolatedInputActions);
            showIsolatedInputActions.target = useIsolatedInputActions.boolValue;
            if (EditorGUILayout.BeginFadeGroup(showIsolatedInputActions.faded))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(pointerDownAction);
                EditorGUILayout.PropertyField(pointerMoveAction);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}