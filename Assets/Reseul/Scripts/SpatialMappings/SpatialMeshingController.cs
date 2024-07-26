// Copyright (c) 2023 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using System.Collections.Generic;
using System.Linq;
using Qualcomm.Snapdragon.Spaces;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Reseul.Snapdragon.Spaces.SpatialMappings
{
    public class SpatialMeshingController : MonoBehaviour
    {
        private ARMeshManager _meshManager;
        private SpacesARMeshManagerConfig _meshManagerConfig;
        public Material SpatialMeshMaterial;

        public void Awake()
        {
            _meshManager = FindObjectOfType<ARMeshManager>(true);
            _meshManagerConfig = FindObjectOfType<SpacesARMeshManagerConfig>(true);

            if (_meshManager == null)
            {
                Debug.LogError("Could not find mesh manager. Sample will not work correctly.");
            }
        }

        public void Start()
        {
        }

        public void OnEnable()
        {
            _meshManager.meshesChanged += OnMeshesChanged;
        }

        public void OnDisable()
        {
            _meshManager.meshesChanged -= OnMeshesChanged;
        }

        private void OnMeshesChanged(ARMeshesChangedEventArgs args)
        {
            if (_meshManagerConfig != null)
            {
                List<Transform> transforms = _meshManager.meshes.ToList().ConvertAll(MeshFilter =>
                {
                    return MeshFilter.transform;
                });
                _meshManagerConfig.UpdateMeshTransforms(args);
            }
        }

        public void EnabledARMesh()
        {
            if (_meshManager != null)
            {
                _meshManager.enabled = true;
            }
        }

        public void VisualizedMesh()
        {
            var color = SpatialMeshMaterial.color;
            SpatialMeshMaterial.color = new Color(color.r, color.g, color.b, 1f);
        }

        public void HideMesh()
        {
            var color = SpatialMeshMaterial.color;
            SpatialMeshMaterial.color = new Color(color.r, color.g, color.b, 0f);
        }

        public void ReleaseARMesh()
        {
            _meshManager.meshesChanged -= OnMeshesChanged;
            _meshManager.DestroyAllMeshes();
            _meshManager.enabled = false;
        }
    }
}