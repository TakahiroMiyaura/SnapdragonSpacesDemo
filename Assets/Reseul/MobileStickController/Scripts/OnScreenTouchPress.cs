// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;

namespace Assets.Reseul.MobileStickController.Scripts
{
    public class OnScreenTouchPress : OnScreenTouchBase, IPointerDownHandler, IPointerUpHandler
    {

        [InputControl(layout = "Button")]
        [SerializeField]
        private string _controlPath;

        protected override string controlPathInternal { 
            get => _controlPath;
            set => _controlPath = value;
        }

        private bool _canEventFire = true;

        public void OnPointerDown(PointerEventData eventData)
        {
            _canEventFire = CanEventFire(eventData);
            if (!_canEventFire) return;
            SendValueToControl(1.0f);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_canEventFire) return;
            SendValueToControl(0.0f);
        }

    }
}