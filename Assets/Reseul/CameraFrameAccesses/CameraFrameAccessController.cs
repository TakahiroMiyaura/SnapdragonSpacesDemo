using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reseul.Snapdragon.Spaces.CameraFrameAccesses
{
    public class CameraFrameAccessController : MonoBehaviour
    {

        public CameraFrameAccess CameraFrameAccessObject;

        public GameObject DisplayObject;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void EnableCameraFrameAccess()
        {
            CameraFrameAccessObject.enabled = true;
            DisplayObject.SetActive(true);
        }
    }
}