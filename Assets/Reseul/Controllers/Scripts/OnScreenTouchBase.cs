// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Qualcomm.Snapdragon.Spaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public abstract class OnScreenTouchBase : OnScreenControl
    {
        [SerializeField]
        protected RectTransform[] Controls;

        [SerializeField]
        protected Camera PhoneCamera;

        private void Awake()
        {
            var spacesHostView = FindObjectOfType<SpacesHostView>(true);
            if (spacesHostView != null && PhoneCamera == null) PhoneCamera = spacesHostView.phoneCamera;
        }

        protected bool CanEventFire(PointerEventData eventData)
        {
            foreach (var rect in Controls)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, eventData.position, PhoneCamera))
                {
                    return false;
                }
            }

            return true;
        }
    }
}