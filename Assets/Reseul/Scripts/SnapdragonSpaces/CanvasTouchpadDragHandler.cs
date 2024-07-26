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
            RectTransform rectTransform = GetComponent<RectTransform>();
            float halfWidth = rectTransform.rect.width / 2;
            float halfHeight = rectTransform.rect.height / 2;

            Vector2 localizedPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventPosition,
                    null, out localizedPosition))
            {
                Vector2 normalized = new Vector2(localizedPosition.x / halfWidth, localizedPosition.y / halfHeight).normalized;
                return normalized;
            }
            return Vector2.zero;
        }

        private CanvasController inputDevice;

        void OnEnable()
        {
            inputDevice = CanvasController.Instance;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            inputDevice.SendTouchpadEvent(1, NormalizedPosition(eventData.position));
        }

        public void OnDrag(PointerEventData eventData)
        {
            inputDevice.SendTouchpadEvent(2, NormalizedPosition(eventData.position));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inputDevice.SendTouchpadEvent(0, NormalizedPosition(eventData.position));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            inputDevice.SendTouchpadEvent(1, NormalizedPosition(eventData.position));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            inputDevice.SendTouchpadEvent(0, NormalizedPosition(eventData.position));
        }

    }
}