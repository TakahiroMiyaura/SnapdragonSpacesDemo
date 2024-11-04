// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;

namespace Reseul.Snapdragon.Spaces.Utilities
{
    public class TransformFollower : MonoBehaviour
    {
        public Transform transformToFollow;

        // Update is called once per frame
        private void Update()
        {
            if (transformToFollow != null)
            {
                transform.position = transformToFollow.position;
                transform.rotation = transformToFollow.rotation;
            }
        }
    }
}