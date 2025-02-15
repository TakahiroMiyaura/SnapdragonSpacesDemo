// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using System.Runtime.InteropServices;
using Reseul.Snapdragon.Spaces.Utilities;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Reseul.Snapdragon.Spaces.CameraFrameAccesses
{
    public class CameraFrameAccess : MonoBehaviour
    {
        [SerializeField]
        private ARCameraManager cameraManager;

        [Header("Camera Feed")]
        [SerializeField]
        private RawImage cameraRawImage;

        [SerializeField]
        private bool renderUsingYUVPlanes;

        private NativeArray<XRCameraConfiguration> cameraConfigs;
        private Texture2D cameraTexture;
        private float defaultAspectRatio;
        private bool deviceSupported;
        private readonly bool feedPaused;

        private XRCpuImage lastCpuImage;
        private Vector2 maxTextureSize;

        private byte[] rgbBuffer;

        public CameraFrameAccess(bool feedPaused, ARCameraManager cameraManager)
        {
            this.feedPaused = feedPaused;
            this.cameraManager = cameraManager;
        }

        public void Awake()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
            }
        }

        private void OnEnable()
        {
            deviceSupported = CheckDeviceSupported();
            if (!deviceSupported)
            {
                OnDeviceNotSupported();
                return;
            }

            if (!CheckSubsystem())
            {
                return;
            }

            deviceSupported = FindSupportedConfiguration();
            if (!deviceSupported)
            {
                OnDeviceNotSupported();
                return;
            }

            if (cameraManager != null)
            {
                cameraManager.frameReceived += OnFrameReceived;
            }

            maxTextureSize = cameraRawImage.rectTransform.sizeDelta;
            defaultAspectRatio = maxTextureSize.x / maxTextureSize.y;
        }

        private void OnDisable()
        {
            if (cameraManager != null)
            {
                cameraManager.frameReceived -= OnFrameReceived;
            }

            lastCpuImage.Dispose();
            cameraConfigs.Dispose();
        }

        // Start is called before the first frame update
        private void Start()
        {
        }

        private void OnFrameReceived(ARCameraFrameEventArgs args)
        {
            if (feedPaused)
            {
                return;
            }

            if (!cameraManager.TryAcquireLatestCpuImage(out lastCpuImage))
            {
                Debug.Log("Failed to acquire latest cpu image.");
                return;
            }

            UpdateCameraTexture(lastCpuImage, renderUsingYUVPlanes);
        }


        private void ResizeCameraFeed(Vector2Int outputDimensions)
        {
            var outputAspectRatio = outputDimensions.x / (float)outputDimensions.y;
            if (outputAspectRatio > defaultAspectRatio)
            {
                cameraRawImage.rectTransform.sizeDelta =
                    new Vector2(maxTextureSize.x, maxTextureSize.x / outputAspectRatio);
            }
            else
            {
                cameraRawImage.rectTransform.sizeDelta =
                    new Vector2(maxTextureSize.y * outputAspectRatio, maxTextureSize.y);
            }
        }

        private unsafe void UpdateCameraTexture(XRCpuImage image, bool convertYuvManually)
        {
            var format = TextureFormat.RGBA32;

            var downsamplingFactor = Mathf.CeilToInt(image.width / 640);
            var outputDimensions = convertYuvManually ? image.dimensions : image.dimensions / downsamplingFactor;

            if (cameraTexture == null || cameraTexture.width != outputDimensions.x ||
                cameraTexture.height != outputDimensions.y)
            {
                cameraTexture = new Texture2D(outputDimensions.x, outputDimensions.y, format, false);
                ResizeCameraFeed(outputDimensions);
            }

            var rawTextureData = cameraTexture.GetRawTextureData<byte>();
            var rawTexturePtr = new IntPtr(rawTextureData.GetUnsafePtr());

            if (convertYuvManually)
            {
                switch (image.planeCount)
                {
                    // YUYV format - 1 plane (YUYV)
                    case 1:
                        ConvertYuyvImageIntoBuffer(image, rawTexturePtr, format);
                        break;
                    // YUV format - 2 planes (Y and UV)
                    case 2:
                        ConvertYuvImageIntoBuffer(image, rawTexturePtr, format);
                        break;
                }
            }
            else
            {
                var conversionParams = new XRCpuImage.ConversionParams(image, format);
                try
                {
                    conversionParams.inputRect = new RectInt(0, 0, image.width, image.height);
                    conversionParams.outputDimensions = outputDimensions;
                    image.Convert(conversionParams, rawTexturePtr, rawTextureData.Length);
                }
                finally
                {
                    image.Dispose();
                }
            }

            cameraTexture.Apply();
            cameraRawImage.texture = cameraTexture;
        }

        private void ConvertYuvImageIntoBuffer(XRCpuImage image, IntPtr targetBuffer, TextureFormat format)
        {
            var bufferSize = image.height * image.width * (format == TextureFormat.RGB24 ? 3 : 4);

            if (rgbBuffer == null || rgbBuffer.Length != bufferSize)
            {
                rgbBuffer = new byte[bufferSize];
            }

            var yPlane = image.GetPlane(0);
            var uvPlane = image.GetPlane(1);

            for (var row = 0; row < image.height; row++)
            {
                for (var col = 0; col < image.width; col++)
                {
                    var y = yPlane.data[row * yPlane.rowStride + col * yPlane.pixelStride];

                    var offset = row / 2 * uvPlane.rowStride + col / 2 * uvPlane.pixelStride;
                    var u = (sbyte)(uvPlane.data[offset] - 128);
                    var v = (sbyte)(uvPlane.data[offset + 1] - 128);

                    // YUV NV12 to RGB conversion
                    // https://en.wikipedia.org/wiki/YUV#Y%E2%80%B2UV420sp_(NV21)_to_RGB_conversion_(Android)
                    var r = y + 1.370705f * v;
                    var g = y - 0.698001f * v - 0.337633f * u;
                    var b = y + 1.732446f * u;

                    r = r > 255 ? 255 : r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b < 0 ? 0 : b;

                    var pixelIndex = (image.height - row - 1) * image.width + col;

                    switch (format)
                    {
                        case TextureFormat.RGB24:
                            rgbBuffer[3 * pixelIndex] = (byte)r;
                            rgbBuffer[3 * pixelIndex + 1] = (byte)g;
                            rgbBuffer[3 * pixelIndex + 2] = (byte)b;
                            break;
                        case TextureFormat.RGBA32:
                            rgbBuffer[4 * pixelIndex] = (byte)r;
                            rgbBuffer[4 * pixelIndex + 1] = (byte)g;
                            rgbBuffer[4 * pixelIndex + 2] = (byte)b;
                            rgbBuffer[4 * pixelIndex + 3] = 255;
                            break;
                        case TextureFormat.BGRA32:
                            rgbBuffer[4 * pixelIndex] = (byte)b;
                            rgbBuffer[4 * pixelIndex + 1] = (byte)g;
                            rgbBuffer[4 * pixelIndex + 2] = (byte)r;
                            rgbBuffer[4 * pixelIndex + 3] = 255;
                            break;
                    }
                }
            }

            Marshal.Copy(rgbBuffer, 0, targetBuffer, bufferSize);
        }

        private void ConvertYuyvImageIntoBuffer(XRCpuImage image, IntPtr targetBuffer, TextureFormat format)
        {
            var bufferSize = image.height * image.width * (format == TextureFormat.RGB24 ? 3 : 4);

            if (rgbBuffer == null || rgbBuffer.Length != bufferSize)
            {
                rgbBuffer = new byte[bufferSize];
            }

            var yuyvPlane = image.GetPlane(0);

            for (var row = 0; row < image.height; row++)
            {
                for (var col = 0; col < image.width; col++)
                {
                    var y = yuyvPlane.data[row * yuyvPlane.rowStride + col * 2];

                    // Calculate offset of the YUYV byte group, select U (2nd byte) and V (4th byte)
                    var offset = row * yuyvPlane.rowStride + col / 2 * yuyvPlane.pixelStride;
                    var u = (sbyte)(yuyvPlane.data[offset + 1] - 128);
                    var v = (sbyte)(yuyvPlane.data[offset + 3] - 128);

                    // YUV NV12 to RGB conversion
                    // https://en.wikipedia.org/wiki/YUV#Y%E2%80%B2UV420sp_(NV21)_to_RGB_conversion_(Android)
                    var r = y + 1.370705f * v;
                    var g = y - 0.698001f * v - 0.337633f * u;
                    var b = y + 1.732446f * u;

                    r = r > 255 ? 255 : r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b < 0 ? 0 : b;

                    var pixelIndex = (image.height - row - 1) * image.width + col;

                    switch (format)
                    {
                        case TextureFormat.RGB24:
                            rgbBuffer[3 * pixelIndex] = (byte)r;
                            rgbBuffer[3 * pixelIndex + 1] = (byte)g;
                            rgbBuffer[3 * pixelIndex + 2] = (byte)b;
                            break;
                        case TextureFormat.RGBA32:
                            rgbBuffer[4 * pixelIndex] = (byte)r;
                            rgbBuffer[4 * pixelIndex + 1] = (byte)g;
                            rgbBuffer[4 * pixelIndex + 2] = (byte)b;
                            rgbBuffer[4 * pixelIndex + 3] = 255;
                            break;
                        case TextureFormat.BGRA32:
                            rgbBuffer[4 * pixelIndex] = (byte)b;
                            rgbBuffer[4 * pixelIndex + 1] = (byte)g;
                            rgbBuffer[4 * pixelIndex + 2] = (byte)r;
                            rgbBuffer[4 * pixelIndex + 3] = 255;
                            break;
                    }
                }
            }

            Marshal.Copy(rgbBuffer, 0, targetBuffer, bufferSize);
        }

        private bool FindSupportedConfiguration()
        {
            cameraConfigs = cameraManager.GetConfigurations(Allocator.Persistent);
            return cameraConfigs.Length > 0;
        }

        private bool CheckDeviceSupported()
        {
            // Currently not supporting Lenovo VRX
            var type = DeviceConfirmProvider.GetCurrentDeviceType();
            return type == XRDeviceType.Handheld;
        }

        private void OnDeviceNotSupported()
        {
            Debug.Log("This feature is not currently supported on this device.");
        }

        private bool CheckSubsystem()
        {
            return cameraManager.subsystem?.running ?? false;
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}