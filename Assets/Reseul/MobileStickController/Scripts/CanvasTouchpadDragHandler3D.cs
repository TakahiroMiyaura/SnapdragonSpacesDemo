// Copyright (c) 2023 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.EventSystems;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class CanvasTouchpadDragHandler3D : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private Camera _arCamera;

        [SerializeField]
        private RectTransform _camCanvas;

        Vector3 NormalizedPosition(Vector2 eventPosition)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_camCanvas, eventPosition, _arCamera,
                    out Vector3 result))
            {
                return result;
            }
            return Vector3.zero;
        }

        private CanvasController inputDevice;

        void OnEnable()
        {
            inputDevice = CanvasController.Instance;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            inputDevice.SendTouchScreenPosition3DEvent(1, NormalizedPosition(eventData.position));
        }

        public void OnDrag(PointerEventData eventData)
        {
            inputDevice.SendTouchScreenPosition3DEvent(2, NormalizedPosition(eventData.position));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inputDevice.SendTouchScreenPosition3DEvent(0, NormalizedPosition(eventData.position));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            inputDevice.SendTouchScreenPosition3DEvent(1, NormalizedPosition(eventData.position));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            inputDevice.SendTouchScreenPosition3DEvent(0, NormalizedPosition(eventData.position));
        }

    }
}