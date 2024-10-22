using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Assets.Reseul.MobileStickController.Scripts
{
    public class ValidateTouchpadController : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference _touchpadDelta;

        [SerializeField]
        private InputActionReference _touchpad;

        [SerializeField]
        private RectTransform _cursor;

        [SerializeField]
        private Camera _arCamera;

        [SerializeField]
        private RectTransform _camCanvas;
        [SerializeField]
        private GameObject _start;

        [SerializeField]
        private GameObject _end;

        private bool _isTouch;

        private Vector2 currentPos= Vector2.zero;

        void OnEnable()
        {
            _touchpadDelta.action.performed += OnTouchpad;
            _touchpadDelta.action.Enable();
            _touchpad.action.started += OnSarted;
            _touchpad.action.Enable();
            //   _touchpadDelta.action.canceled += OnTouchpadCancel;
        }

        private void OnSarted(InputAction.CallbackContext obj)
        {
            currentPos = _touchpad.action.ReadValue<Vector2>();
        }

        private void OnTouchpadCancel(InputAction.CallbackContext obj)
        {
            _isTouch = false;
        }

        //void Update()
        //{
        //    if (_isTouch)
        //    {
        //        CameraBasePosition(_touchpadDelta.action.ReadValue<Vector2>());
        //    }
        //}

        private void OnTouchpad(InputAction.CallbackContext obj)
        {
            if (obj.phase != InputActionPhase.Performed)
            {
                return;
            }

            currentPos += obj.action.ReadValue<Vector2>();

            CameraBasePosition(currentPos);
            _isTouch = true;
        }

        private void WorldPos(Vector2 touchpadValue)
        {

            var rect = _cursor.GetComponent<RectTransform>();

            var pos = RectTransformUtility.WorldToScreenPoint(_arCamera, rect.position);

            RectTransformUtility.ScreenPointToWorldPointInRectangle(_camCanvas, pos, _arCamera, out Vector3 result);
            _start.transform.position = _camCanvas.gameObject.transform.position + result + _arCamera.transform.forward;
            _end.transform.position = _camCanvas.gameObject.transform.position + result - _arCamera.transform.forward*10f;

            RaycastHit hit;
            if (Physics.Raycast(new Ray(_end.transform.position, _end.transform.forward), out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
            }
            else if (Physics.Raycast(RectTransformUtility.ScreenPointToRay(_arCamera, rect.position), out hit))
            {
                Debug.Log($"hit:{hit.collider.gameObject.name}");
            }
        }

        private void CameraBasePosition(Vector2 touchpadValue)
        {

            _cursor.anchoredPosition = touchpadValue;
            var rect = _cursor.GetComponent<RectTransform>();
            
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_camCanvas, touchpadValue, _arCamera, out Vector3 result);
            _start.transform.position = result ;
            _end.transform.position = result ;


            var ray = RectTransformUtility.ScreenPointToRay(_arCamera, touchpadValue);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"hit:{hit.collider.gameObject.name}");
            }
        }

        void OnDisable()
        {
            _touchpadDelta.action.performed -= OnTouchpad;
            _touchpadDelta.action.Disable();
            _touchpad.action.started -= OnSarted;
            _touchpad.action.Disable();
        }
    }
}
