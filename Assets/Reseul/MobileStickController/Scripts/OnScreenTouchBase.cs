// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Qualcomm.Snapdragon.Spaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Assets.Reseul.MobileStickController.Scripts
{
    public abstract class OnScreenTouchBase : OnScreenControl
    {
        void Awake()
        {
            var spacesHostView = FindObjectOfType<SpacesHostView>(true);
            if (spacesHostView != null && _phoneCamera == null) _phoneCamera = spacesHostView.phoneCamera;
        }
        [SerializeField]
        protected RectTransform[] _controls;

        [SerializeField]
        protected Camera _phoneCamera;

        protected bool CanEventFire(PointerEventData eventData)
        {
            foreach (var rect in _controls)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, eventData.position,_phoneCamera))
                {
                    return false;
                }
            }

            return true;
        }

    }
}