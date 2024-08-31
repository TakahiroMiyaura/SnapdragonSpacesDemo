// Copyright (c) 2023 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class CanvasControllerButton1 : Button
    {
        private CanvasController inputDevice;

        protected override void OnEnable()
        {
            base.OnEnable();
            inputDevice = CanvasController.Instance;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            inputDevice.SendButton1Event(1);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            inputDevice.SendButton1Event(0);
        }
        
    }
    

}