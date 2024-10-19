// Copyright (c) 2023 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.EventSystems;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class CanvasTouchpadDragHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        
        Vector2 NormalizedPosition(Vector2 eventPosition)
        {
            return eventPosition;
        }

        private CanvasController inputDevice;

        void OnEnable()
        {
            inputDevice = CanvasController.Instance;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            inputDevice.SendTouchScreenPositionEvent(1, NormalizedPosition(eventData.position));
        }

        public void OnDrag(PointerEventData eventData)
        {
            inputDevice.SendTouchScreenPositionEvent(2, NormalizedPosition(eventData.position));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inputDevice.SendTouchScreenPositionEvent(0, NormalizedPosition(eventData.position));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            inputDevice.SendTouchScreenPositionEvent(1, NormalizedPosition(eventData.position));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            inputDevice.SendTouchScreenPositionEvent(0, NormalizedPosition(eventData.position));
        }

    }
}