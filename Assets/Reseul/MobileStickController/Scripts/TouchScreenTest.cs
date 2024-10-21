using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.MixedReality.GraphicsTools;
using Reseul.Snapdragon.Spaces.Controllers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Assets.Reseul.MobileStickController.Scripts
{
    public class TouchScreenTest: OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField]
        [InputControl(layout = "Vector2")]
        private string _controlPath;

        private CanvasElementRoundedRect _canvasElementRoundedRect;


        protected override string controlPathInternal
        {
            get => _controlPath;
            set => _controlPath = value;
        }

        void OnStart()
        {
            _canvasElementRoundedRect = GetComponentInChildren<CanvasElementRoundedRect>();
            _canvasElementRoundedRect.enabled=false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            CanvasController.Instance.SendTouchRadiusEvent(1,eventData.radius);
            CanvasController.Instance.SendTouchScreenPositionEvent(1, eventData.position);
            _canvasElementRoundedRect.transform.position = eventData.position;
            _canvasElementRoundedRect.enabled = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            CanvasController.Instance.SendTouchRadiusEvent(0, eventData.radius);
            CanvasController.Instance.SendTouchScreenPositionEvent(0, eventData.position);
            _canvasElementRoundedRect.transform.position = eventData.position;
            _canvasElementRoundedRect.enabled = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            CanvasController.Instance.SendTouchScreenDeltaEvent(1, eventData.delta);
        }
    }
}
