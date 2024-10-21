using Qualcomm.Snapdragon.Spaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Assets.Reseul.MobileStickController.Scripts
{
    public class OnScreenTouch : OnScreenTouchBase, IPointerDownHandler, IPointerUpHandler,IDragHandler
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
            SendValueToControl(eventData.position);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_canEventFire) return;
            SendValueToControl(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_canEventFire) return;
            SendValueToControl(eventData.position);
        }
    }
}
