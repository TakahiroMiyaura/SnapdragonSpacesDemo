// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;

namespace Assets.Reseul.MobileStickController.Scripts
{
    public class OnScreenTouch3D : OnScreenTouchBase, IPointerDownHandler, IPointerUpHandler,IDragHandler
    {
        private bool _canEventFire;

        [InputControl(layout = "Vector3")]
        [SerializeField]
        private string _controlPath;

        [SerializeField]
        private RectTransform _camCanvas;

        protected override string controlPathInternal
        {
            get => _controlPath;
            set => _controlPath = value;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _canEventFire = CanEventFire(eventData);
            if (!_canEventFire) return;
            SendValueToControl(eventData);
        }

        private void SendValueToControl(PointerEventData eventData)
        {
            var pos = RectTransformUtility.WorldToScreenPoint(_phoneCamera, eventData.position);

            RectTransformUtility.ScreenPointToWorldPointInRectangle(_camCanvas, pos, _phoneCamera, out Vector3 result);
            SendValueToControl(result);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_canEventFire) return;
            SendValueToControl(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_canEventFire) return;
            SendValueToControl(eventData);
        }
    }
}