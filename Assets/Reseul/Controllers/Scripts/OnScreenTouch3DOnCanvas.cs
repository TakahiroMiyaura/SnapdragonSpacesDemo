// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class OnScreenTouch3DOnCanvas : OnScreenTouchBase, IPointerDownHandler, IPointerUpHandler,IDragHandler
    {
        private bool canEventFire;

        [InputControl(layout = "Vector3")]
        [SerializeField]
        private string touchScreenControlPath;

        [SerializeField]
        private RectTransform targetRectTransform;

        [SerializeField]
        private float distanceFromCanvas = 0f;

        protected override string controlPathInternal
        {
            get => touchScreenControlPath;
            set => touchScreenControlPath = value;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            canEventFire = CanEventFire(eventData);
            if (!canEventFire) return;
            SendValueToControl(eventData);
        }

        private void SendValueToControl(PointerEventData eventData)
        {
            var result = Calculate3DPositionOnCanvasFrom2D(eventData.position);
            SendValueToControl(result);
        }

        internal Vector3 Calculate3DPositionOnCanvasFrom2D(Vector2 eventDataPosition)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(targetRectTransform, eventDataPosition, null, out var result);
            result -= targetRectTransform.forward * distanceFromCanvas;
            return result;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!canEventFire) return;
            SendValueToControl(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!canEventFire) return;
            SendValueToControl(eventData);
        }
    }
}