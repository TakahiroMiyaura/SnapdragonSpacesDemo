using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qualcomm.Snapdragon.Spaces;
using Reseul.Snapdragon.Spaces.Controllers;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.UIElements;

namespace Assets.Reseul.MobileStickController.Scripts
{
    public class TouchScreenConvertHandler : MonoBehaviour
    {
        private CanvasControllerInputDeviceState _companionState;
        private Camera _phoneCamera;

        [SerializeField]
        private Canvas _canvas;

        private Vector2 _radius = Vector2.negativeInfinity;

        private void Start()
        {
            var spacesHostView = FindObjectOfType<SpacesHostView>(true);
            if(spacesHostView != null)
            {
                _phoneCamera = spacesHostView.phoneCamera;
            }
        }

        // Touch #0 入力
        public void OnPrimaryTouch(InputAction.CallbackContext context)
        {
            var touchState = context.ReadValue<TouchState>();

            CanvasController.Instance.SendTouchScreenPositionEvent((int)TouchPhase.Began, touchState.position);
        }

        public void OnPrimaryDelta(InputAction.CallbackContext context)
        {
            var delta = context.ReadValue<Vector2>();

            CanvasController.Instance.SendTouchScreenDeltaEvent((int)TouchPhase.Moved, delta);
        }
        public void OnPrimaryPosition(InputAction.CallbackContext context)
        {
            var position = context.ReadValue<Vector2>();

            CanvasController.Instance.SendTouchScreenPositionEvent((int)TouchPhase.Began, position);
        }

        public void OnPrimaryContact(InputAction.CallbackContext context)
        {
            var contact = context.ReadValue<TouchPhase>();

            CanvasController.Instance.SendTouchScreenPressEvent(1, contact);

        }

        public void OnPrimaryRadius(InputAction.CallbackContext context)
        {
            var current = context.ReadValue<Vector2>();

            CanvasController.Instance.SendTouchRadiusEvent((int)TouchPhase.Began, _radius);

            if (!float.IsNegativeInfinity(_radius.x))
            {
                var delta = current - _radius;
                CanvasController.Instance.SendTouchRadiusDeltaEvent((int)TouchPhase.Moved, delta);
            }

            _radius = current;
        }
    }
}
