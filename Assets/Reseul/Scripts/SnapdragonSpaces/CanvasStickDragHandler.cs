// Copyright (c) 2023 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Reseul.Snapdragon.Spaces.Controllers
{

    public enum StirckType
    {
        Left,
        Right
    }


    public class CanvasStickDragHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        [SerializeField]
        private AnimationCurve _sensitivity;

        [SerializeField]
        private StirckType type;

        [SerializeField]
        private RectTransform stickTransform;

        [SerializeField]
        private bool isResetPosition = true;

        private CanvasController inputDevice;
        private float halfWidth;
        private float halfHeight;

        void OnEnable()
        {
            inputDevice = CanvasController.Instance;
        }

        Vector2 NormalizedPosition(Vector2 eventPosition)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            halfWidth = rectTransform.rect.width / 2;
            halfHeight = rectTransform.rect.height / 2;

            Vector2 localizedPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventPosition,
                    null, out localizedPosition))
            {
                Vector2 normalized = new Vector2(localizedPosition.x / halfWidth > 1 ? 1.0f : localizedPosition.x / halfWidth < -1 ? -1.0f : localizedPosition.x / halfWidth
                    , localizedPosition.y / halfHeight > 1 ? 1.0f : localizedPosition.y / halfHeight < -1 ? -1.0f : localizedPosition.y / halfHeight);
                if (normalized.sqrMagnitude > 1)
                {
                    normalized = normalized.normalized;
                }
                if(stickTransform != null)
                    stickTransform.anchoredPosition =
                        new Vector2(normalized.x * halfWidth, normalized.y * halfHeight); 

                normalized.x = _sensitivity.Evaluate(normalized.x);
                normalized.y = _sensitivity.Evaluate(normalized.y);
                return normalized;
            }
            return Vector2.zero;
        }

        void Update()
        {
        }

        private void SendStickEvent(PointerEventData eventData, int phase)
        {
            SendStickEvent(NormalizedPosition(eventData.position), phase);
        }

        private void SendStickEvent(Vector2 position, int phase)
        {
            switch (type)
            {
                case StirckType.Left:
                    inputDevice.SendLeftStickEvent(phase, position);
                    break;
                case StirckType.Right:
                    inputDevice.SendRightStickEvent(phase, position);
                    break;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            SendStickEvent(eventData, 1);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SendStickEvent(eventData, 2);
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            if (isResetPosition)
            {
                if (stickTransform != null)
                    stickTransform.anchoredPosition = Vector2.zero;
                SendStickEvent(Vector2.zero, 0);
            }
            else
            {
                SendStickEvent(eventData, 0);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SendStickEvent(eventData, 1);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isResetPosition)
            {
                if (stickTransform != null)
                    stickTransform.anchoredPosition = Vector2.zero;
                SendStickEvent(Vector2.zero, 0);
            }
            else
            {
                SendStickEvent(eventData, 0);
            }
        }
        
    }
}