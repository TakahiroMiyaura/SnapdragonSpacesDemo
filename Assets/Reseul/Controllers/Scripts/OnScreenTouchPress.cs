// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class OnScreenTouchPress : OnScreenTouchBase, IPointerDownHandler, IPointerUpHandler
    {

        [InputControl(layout = "Button")]
        [SerializeField]
        private string touchScreenControlPath;

        protected override string controlPathInternal { 
            get => touchScreenControlPath;
            set => touchScreenControlPath = value;
        }

        private bool canEventFire = true;

        public void OnPointerDown(PointerEventData eventData)
        {
            canEventFire = CanEventFire(eventData);
            if (!canEventFire) return;
            SendValueToControl(1.0f);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!canEventFire) return;
            SendValueToControl(0.0f);
        }

    }
}