// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Qualcomm.Snapdragon.Spaces;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Reseul.Snapdragon.Spaces.SpatialMappings
{
    public class SpatialMeshingController : MonoBehaviour
    {
        [SerializeField]
        private Material spatialMeshMaterial;

        private ARMeshManager meshManager;
        private SpacesARMeshManagerConfig meshManagerConfig;

        public void Awake()
        {
            meshManager = FindObjectOfType<ARMeshManager>(true);
            meshManagerConfig = FindObjectOfType<SpacesARMeshManagerConfig>(true);

            if (meshManager == null)
            {
                Debug.LogError("Could not find mesh manager. Sample will not work correctly.");
            }
        }

        public void Start()
        {
        }

        public void OnEnable()
        {
            meshManager.meshesChanged += OnMeshesChanged;
        }

        public void OnDisable()
        {
            meshManager.meshesChanged -= OnMeshesChanged;
        }

        private void OnMeshesChanged(ARMeshesChangedEventArgs args)
        {
            meshManagerConfig?.UpdateMeshTransforms(args);
        }

        public void EnabledARMesh()
        {
            if (meshManager != null)
            {
                meshManager.enabled = true;
            }
        }

        public void VisualizedMesh()
        {
            var color = spatialMeshMaterial.color;
            spatialMeshMaterial.color = new Color(color.r, color.g, color.b, 1f);
        }

        public void HideMesh()
        {
            var color = spatialMeshMaterial.color;
            spatialMeshMaterial.color = new Color(color.r, color.g, color.b, 0f);
        }

        public void ReleaseARMesh()
        {
            meshManager.meshesChanged -= OnMeshesChanged;
            meshManager.DestroyAllMeshes();
            meshManager.enabled = false;
        }
    }
}