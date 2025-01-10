using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reseul.Snapdragon.Spaces.CameraFrameAccesses
{
    public class CameraFrameAccessController : MonoBehaviour
    {
        [SerializeField]
        private CameraFrameAccess cameraFrameAccessObject;

        [SerializeField]
        private GameObject displayObject;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void DisableCameraFrameAccess()
        {
            cameraFrameAccessObject.enabled = false;
            displayObject.SetActive(false);
        }

        public void EnableCameraFrameAccess()
        {
            cameraFrameAccessObject.enabled = true;
            displayObject.SetActive(true);
        }
    }
}