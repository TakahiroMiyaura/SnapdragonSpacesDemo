// Copyright (c) 2023 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using System;
using System.Runtime.InteropServices;
using Reseul.Snapdragon.Spaces.Devices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Reseul.Snapdragon.Spaces.CameraFrameAccesses
{
    public class CameraFrameAccess : MonoBehaviour
    {

        [Header("Camera Feed")]
        public RawImage CameraRawImage;

        public bool RenderUsingYUVPlanes;

        [SerializeField]
        private ARCameraManager _cameraManager;
        private Texture2D _cameraTexture;
        private bool _deviceSupported;
        private bool _feedPaused;
        private NativeArray<XRCameraConfiguration> _cameraConfigs;

        private XRCpuImage _lastCpuImage;
        private Vector2 _maxTextureSize;
        private float _defaultAspectRatio;

        private byte[] _rgbBuffer;

        public void Awake()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
            }
        }

        void OnEnable()
        {
            _deviceSupported = CheckDeviceSupported();
            if (!_deviceSupported)
            {
                OnDeviceNotSupported();
                return;
            }

            if (!CheckSubsystem())
            {
                return;
            }

            _deviceSupported = FindSupportedConfiguration();
            if (!_deviceSupported)
            {
                OnDeviceNotSupported();
                return;
            }

            if (_cameraManager != null)
            {
                _cameraManager.frameReceived += OnFrameReceived;
            }

            _maxTextureSize = CameraRawImage.rectTransform.sizeDelta;
            _defaultAspectRatio = _maxTextureSize.x / _maxTextureSize.y;
        }

        void OnDisable()
        {
            if (_cameraManager != null)
            {
                _cameraManager.frameReceived -= OnFrameReceived;
            }
            _lastCpuImage.Dispose();
            _cameraConfigs.Dispose();
        }

        // Start is called before the first frame update
        private void Start()
        {

        }

        private void OnFrameReceived(ARCameraFrameEventArgs args)
        {
            if (_feedPaused)
            {
                return;
            }

            if (!_cameraManager.TryAcquireLatestCpuImage(out _lastCpuImage))
            {
                Debug.Log("Failed to acquire latest cpu image.");
                return;
            }

            UpdateCameraTexture(_lastCpuImage, RenderUsingYUVPlanes);
        }


        private void ResizeCameraFeed(Vector2Int outputDimensions)
        {
            var outputAspectRatio = outputDimensions.x / (float)outputDimensions.y;
            if (outputAspectRatio > _defaultAspectRatio)
            {
                CameraRawImage.rectTransform.sizeDelta = new Vector2(_maxTextureSize.x, _maxTextureSize.x / outputAspectRatio);
            }
            else
            {
                CameraRawImage.rectTransform.sizeDelta = new Vector2(_maxTextureSize.y * outputAspectRatio, _maxTextureSize.y);
            }
        }

        private unsafe void UpdateCameraTexture(XRCpuImage image, bool convertYuvManually)
        {
            var format = TextureFormat.RGBA32;

            var downsamplingFactor = Mathf.CeilToInt(image.width / 640);
            var outputDimensions = convertYuvManually ? image.dimensions : image.dimensions / downsamplingFactor;

            if (_cameraTexture == null || _cameraTexture.width != outputDimensions.x || _cameraTexture.height != outputDimensions.y)
            {
                _cameraTexture = new Texture2D(outputDimensions.x, outputDimensions.y, format, false);
                ResizeCameraFeed(outputDimensions);
            }

            var rawTextureData = _cameraTexture.GetRawTextureData<byte>();
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

            _cameraTexture.Apply();
            CameraRawImage.texture = _cameraTexture;
        }

        private void ConvertYuvImageIntoBuffer(XRCpuImage image, IntPtr targetBuffer, TextureFormat format)
        {
            var bufferSize = image.height * image.width * (format == TextureFormat.RGB24 ? 3 : 4);

            if (_rgbBuffer == null || _rgbBuffer.Length != bufferSize)
            {
                _rgbBuffer = new byte[bufferSize];
            }

            var yPlane = image.GetPlane(0);
            var uvPlane = image.GetPlane(1);

            for (int row = 0; row < image.height; row++)
            {
                for (int col = 0; col < image.width; col++)
                {
                    var y = yPlane.data[row * yPlane.rowStride + col * yPlane.pixelStride];

                    var offset = (row / 2) * uvPlane.rowStride + (col / 2) * uvPlane.pixelStride;
                    sbyte u = (sbyte)(uvPlane.data[offset] - 128);
                    sbyte v = (sbyte)(uvPlane.data[offset + 1] - 128);

                    // YUV NV12 to RGB conversion
                    // https://en.wikipedia.org/wiki/YUV#Y%E2%80%B2UV420sp_(NV21)_to_RGB_conversion_(Android)
                    var r = y + (1.370705f * v);
                    var g = y - (0.698001f * v) - (0.337633f * u);
                    var b = y + (1.732446f * u);

                    r = r > 255 ? 255 : r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b < 0 ? 0 : b;

                    int pixelIndex = ((image.height - row - 1) * image.width) + col;

                    switch (format)
                    {
                        case TextureFormat.RGB24:
                            _rgbBuffer[3 * pixelIndex] = (byte)r;
                            _rgbBuffer[(3 * pixelIndex) + 1] = (byte)g;
                            _rgbBuffer[(3 * pixelIndex) + 2] = (byte)b;
                            break;
                        case TextureFormat.RGBA32:
                            _rgbBuffer[4 * pixelIndex] = (byte)r;
                            _rgbBuffer[(4 * pixelIndex) + 1] = (byte)g;
                            _rgbBuffer[(4 * pixelIndex) + 2] = (byte)b;
                            _rgbBuffer[(4 * pixelIndex) + 3] = 255;
                            break;
                        case TextureFormat.BGRA32:
                            _rgbBuffer[4 * pixelIndex] = (byte)b;
                            _rgbBuffer[(4 * pixelIndex) + 1] = (byte)g;
                            _rgbBuffer[(4 * pixelIndex) + 2] = (byte)r;
                            _rgbBuffer[(4 * pixelIndex) + 3] = 255;
                            break;
                    }
                }
            }
            Marshal.Copy(_rgbBuffer, 0, targetBuffer, bufferSize);
        }
        private void ConvertYuyvImageIntoBuffer(XRCpuImage image, IntPtr targetBuffer, TextureFormat format)
        {
            var bufferSize = image.height * image.width * (format == TextureFormat.RGB24 ? 3 : 4);

            if (_rgbBuffer == null || _rgbBuffer.Length != bufferSize)
            {
                _rgbBuffer = new byte[bufferSize];
            }

            var yuyvPlane = image.GetPlane(0);

            for (int row = 0; row < image.height; row++)
            {
                for (int col = 0; col < image.width; col++)
                {
                    var y = yuyvPlane.data[row * yuyvPlane.rowStride + col * 2];

                    // Calculate offset of the YUYV byte group, select U (2nd byte) and V (4th byte)
                    var offset = row * yuyvPlane.rowStride + (col / 2) * yuyvPlane.pixelStride;
                    sbyte u = (sbyte)(yuyvPlane.data[offset + 1] - 128);
                    sbyte v = (sbyte)(yuyvPlane.data[offset + 3] - 128);

                    // YUV NV12 to RGB conversion
                    // https://en.wikipedia.org/wiki/YUV#Y%E2%80%B2UV420sp_(NV21)_to_RGB_conversion_(Android)
                    var r = y + (1.370705f * v);
                    var g = y - (0.698001f * v) - (0.337633f * u);
                    var b = y + (1.732446f * u);

                    r = r > 255 ? 255 : r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b < 0 ? 0 : b;

                    int pixelIndex = ((image.height - row - 1) * image.width) + col;

                    switch (format)
                    {
                        case TextureFormat.RGB24:
                            _rgbBuffer[3 * pixelIndex] = (byte)r;
                            _rgbBuffer[(3 * pixelIndex) + 1] = (byte)g;
                            _rgbBuffer[(3 * pixelIndex) + 2] = (byte)b;
                            break;
                        case TextureFormat.RGBA32:
                            _rgbBuffer[4 * pixelIndex] = (byte)r;
                            _rgbBuffer[(4 * pixelIndex) + 1] = (byte)g;
                            _rgbBuffer[(4 * pixelIndex) + 2] = (byte)b;
                            _rgbBuffer[(4 * pixelIndex) + 3] = 255;
                            break;
                        case TextureFormat.BGRA32:
                            _rgbBuffer[4 * pixelIndex] = (byte)b;
                            _rgbBuffer[(4 * pixelIndex) + 1] = (byte)g;
                            _rgbBuffer[(4 * pixelIndex) + 2] = (byte)r;
                            _rgbBuffer[(4 * pixelIndex) + 3] = 255;
                            break;
                    }
                }
            }
            Marshal.Copy(_rgbBuffer, 0, targetBuffer, bufferSize);
        }

        private bool FindSupportedConfiguration()
        {
            _cameraConfigs = _cameraManager.GetConfigurations(Allocator.Persistent);
            return _cameraConfigs.Length > 0;
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
            return _cameraManager.subsystem?.running ?? false;
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}