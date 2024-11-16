// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Unity.XR.CoreUtils;
using UnityEngine;

namespace Reseul.Snapdragon.Spaces.Controllers
{
    public class MobileStickController : MonoBehaviour
    {
        private static MobileStickController instance;

        [SerializeField]
        private RectTransform rayCastTarget;

        [SerializeField]
        private bool visualControllerInfo = true;

        private GameObject debugObject;

        public static MobileStickController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<MobileStickController>();
                }

                return instance;
            }
        }

        public RectTransform RayCastTarget => rayCastTarget;

        // Start is called before the first frame update
        private void Start()
        {
            foreach (var canvas in gameObject.GetComponentsInChildren<Canvas>(true))
            {
                if ((canvas.isRootCanvas && canvas.renderMode == RenderMode.WorldSpace)
                    || !canvas.isRootCanvas)
                {
                    canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
                }
            }

            debugObject = gameObject.GetNamedChild("Debug");
        }

        // Update is called once per frame
        private void Update()
        {
            debugObject?.SetActive(visualControllerInfo);
        }
    }
}