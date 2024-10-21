// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;

namespace Assets.Reseul.MobileStickController.Scripts
{
    public class OnScreenTouchDelta : OnScreenTouchBase, IPointerDownHandler, IPointerUpHandler,IDragHandler
    {
        private bool _canEventFire;

        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string _controlPath;

        protected override string controlPathInternal
        {
            get => _controlPath;
            set => _controlPath = value;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _canEventFire = CanEventFire(eventData);
            if (!_canEventFire) return;
            SendValueToControl(eventData.delta);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_canEventFire) return;
            SendValueToControl(eventData.delta);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_canEventFire) return;
            SendValueToControl(eventData.delta);
        }
    }
}