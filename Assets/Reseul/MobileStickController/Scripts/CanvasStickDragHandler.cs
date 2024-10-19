// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Unity.VisualScripting;
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

    public class CanvasStickDragHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler,
        IDragHandler, IEndDragHandler
    {
        private Vector2 _dirtyPos;
        private Vector2 _prev = Vector2.negativeInfinity;

        [SerializeField]
        private AnimationCurve _sensitivity;

        private float halfHeight;
        private float halfWidth;

        private CanvasController inputDevice;

        [SerializeField]
        private readonly bool isResetPosition = true;

        [SerializeField]
        private RectTransform stickTransform;

        [SerializeField]
        private StirckType type;

        private RectTransform _currentRectTransform;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dirtyPos = eventData.position;
            SendStickEvent(eventData, 1);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SendStickDeltaEvent(eventData, 2);
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
            OnBeginDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnEndDrag(eventData);
        }

        private void OnEnable()
        {
            inputDevice = CanvasController.Instance;
            _currentRectTransform = GetComponent<RectTransform>();
            halfWidth = _currentRectTransform.rect.width / 2;
            halfHeight = _currentRectTransform.rect.height / 2;
        }

        private void SetStickTransform(Vector2 position)
        {
            if (stickTransform != null)
                stickTransform.anchoredPosition =
                new Vector2(position.x * halfWidth, position.y * halfHeight);
        }

        private Vector2 NormalizedPosition(Vector2 eventPosition)
        {
            Vector2 localizedPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventPosition,
                    null, out localizedPosition))
            {
                var normalized = new Vector2(
                    localizedPosition.x / halfWidth > 1 ? 1.0f :
                    localizedPosition.x / halfWidth < -1 ? -1.0f : localizedPosition.x / halfWidth
                    , localizedPosition.y / halfHeight > 1 ? 1.0f : localizedPosition.y / halfHeight < -1 ? -1.0f : localizedPosition.y / halfHeight);
                normalized.x = _sensitivity.Evaluate(normalized.x);
                normalized.y = _sensitivity.Evaluate(normalized.y);
                if (normalized.sqrMagnitude > 1) normalized = normalized.normalized;
                return normalized;
            }

            return Vector2.zero;
        }

        private void Update()
        {
        }

        private void SendStickEvent(PointerEventData eventData, int phase)
        {
            SetStickTransform(eventData.position);
            SendStickEvent(NormalizedPosition(eventData.position), phase);
        }

        private void SendStickEvent(Vector2 position, int phase)
        {
            switch (type)
            {
                case StirckType.Left:
                    inputDevice.SendLeftStickPositionEvent(phase, position);
                    break;
                case StirckType.Right:
                    inputDevice.SendRightStickPositionEvent(phase, position);
                    break;
            }
        }

        private void SendStickDeltaEvent(PointerEventData eventData, int phase)
        {
            var currentPos = NormalizedPosition(eventData.delta + _dirtyPos);
            var delta = currentPos - NormalizedPosition(_dirtyPos);
            SetStickTransform(currentPos);
            _dirtyPos += eventData.delta;
            SendStickDeltaEvent(delta, phase);
        }

        private void SendStickDeltaEvent(Vector2 position, int phase)
        {
            switch (type)
            {
                case StirckType.Left:
                    inputDevice.SendLeftStickDeltaEvent(phase, position);
                    break;
                case StirckType.Right:
                    inputDevice.SendRightStickDeltaEvent(phase, position);
                    break;
            }
        }
    }
}