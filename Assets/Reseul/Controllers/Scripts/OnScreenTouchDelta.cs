// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class OnScreenTouchDelta : OnScreenTouchBase, IPointerDownHandler, IPointerUpHandler,IDragHandler
    {
        private bool canEventFire;

        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string touchScreenControlPath;

        protected override string controlPathInternal
        {
            get => touchScreenControlPath;
            set => touchScreenControlPath = value;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            canEventFire = CanEventFire(eventData);
            if (!canEventFire) return;
            SendValueToControl(eventData.delta);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!canEventFire) return;
            SendValueToControl(eventData.delta);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!canEventFire) return;
            SendValueToControl(eventData.delta);
        }
    }
}