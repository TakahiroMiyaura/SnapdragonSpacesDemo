using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.CameraFrameAccesses
{
    public class CubeManipulation : MonoBehaviour
    {

        public InputActionReference position;
        public InputActionReference rotation;

        public float amp = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var posValue = position.action.ReadValue<Vector2>();
            var rotValue = rotation.action.ReadValue<Vector2>();

            if (position.action.IsPressed())
                transform.position += new Vector3(posValue.x, posValue.y, 0) * amp;
            if (rotation.action.IsPressed())
                transform.rotation *= Quaternion.Euler(rotValue.x, rotValue.y, 0);

        }
    }
}
