// Copyright (c) 2023 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Reseul.Snapdragon.Spaces.CameraFrameAccesses
{


    public class TransformFollower : MonoBehaviour
    {

        public Transform transformToFollow;

        // Update is called once per frame
        void Update()
        {
            if (transformToFollow != null)
            {
                transform.position = transformToFollow.position;
                transform.rotation = transformToFollow.rotation;
            }
        }
    }
}