// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class OnScreenTouch3DOnCanvas : OnScreenTouchBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField]
        private float distanceFromCanvas = 0f;

        [SerializeField]
        private RectTransform targetRectTransform;

        [InputControl(layout = "Vector3")]
        [SerializeField]
        private string touchScreenControlPath;

        private bool canEventFire;

        protected override string controlPathInternal
        {
            get => touchScreenControlPath;
            set => touchScreenControlPath = value;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!canEventFire) return;
            SendValueToControl(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            canEventFire = CanEventFire(eventData);
            if (!canEventFire) return;
            SendValueToControl(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
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
            RectTransformUtility.ScreenPointToWorldPointInRectangle(targetRectTransform, eventDataPosition, null,
                out var result);
            result -= targetRectTransform.forward * distanceFromCanvas;
            return result;
        }
    }
}