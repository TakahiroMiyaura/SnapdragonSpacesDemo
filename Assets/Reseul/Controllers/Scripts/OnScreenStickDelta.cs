// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
#if UNITY_EDITOR
using UnityEditor.AnimatedValues;
#endif

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class OnScreenStickDelta : OnScreenStick, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [InputControl(layout = "Delta")]
        [SerializeField]
        private string controlPathDelta;

        private InputControl controlDelta;

        public new void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            InputSystem.QueueDeltaStateEvent(controlDelta, eventData.delta, Time.realtimeSinceStartup);
        }

        public new void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            InputSystem.QueueDeltaStateEvent(controlDelta, eventData.delta, Time.realtimeSinceStartup);
        }

        public new void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            InputSystem.QueueDeltaStateEvent(controlDelta, Vector2.zero, Time.realtimeSinceStartup);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            controlDelta = InputControlPath.TryFindControl(control.device, controlPathDelta);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(OnScreenStickDelta))]
    internal class OnScreenStickDeltaEditor : Editor
    {
        private SerializedProperty behaviour;
        private SerializedProperty controlPathDelta;
        private SerializedProperty controlPathInternal;
        private SerializedProperty dynamicOriginRange;
        private SerializedProperty movementRange;
        private SerializedProperty pointerDownAction;
        private SerializedProperty pointerMoveAction;
        private AnimBool showDynamicOriginOptions;
        private AnimBool showIsolatedInputActions;

        private SerializedProperty useIsolatedInputActions;

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
            EditorGUILayout.PropertyField(controlPathInternal, new GUIContent("Touch Screen Control Path"));
            EditorGUILayout.PropertyField(controlPathDelta, new GUIContent("Touch Screen Delta Control Path"));
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