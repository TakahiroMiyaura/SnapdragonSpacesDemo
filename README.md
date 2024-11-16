# Snapdragon Spaces Platform Demo Application

This is a sample application that allows you to verify the functionality of Snapdragon Spaces.

[![Demo Video](https://img.youtube.com/vi/dyjCIETCc6c/0.jpg)](https://www.youtube.com/watch?v=dyjCIETCc6c)

## Summary
A stick controller displayed on a smartphone can be used to control a character in real space.Spaces are spatially recognized by AR Foundation.It uses the Dynamic OpenXR Loader, which works as a smartphone app when launched, and enables OpenXR after connecting the AR glasses to enable the AR experience.

- Functions realized on a smartphone by Dual Render Fusion
    - Mesh display of the space analyzed by the device in the same spatial coordinates as AR glasses
    - Virtual controller for character manipulation
    - Display of the image being viewed on the AR glasses side
    - Various debugging information
- Supported Interactions
    - Hand Tracking(of MRTK3 feature)
    - Touch Screen interface
- Snapdragon Spaces features used primarily
    - Dual Render Fusion
    - Spatial Meshing
    - Camera Frame Access
    - Dynamic OpenXR Loader
- Addtional Libraries
    - [Mixed Reality Toolkit 3(MRTK3)](https://github.com/MixedRealityToolkit/MixedRealityToolkit-Unity)

## Develop Environments

- Unity 2023.3.36f1
- Snapdragon Spaces SDk V1.0.1
- Mixed Reality Toolkit 3
   - org.mixedrealitytoolkit.core: 3.2.1
   - org.mixedrealitytoolkit.input: 3.2.1
   - org.mixedrealitytoolkit.spatialmanipulation: 3.3.0
   - org.mixedrealitytoolkit.standardassets: 3.2.0
   - org.mixedrealitytoolkit.uxcomponents: 3.3.0
   - org.mixedrealitytoolkit.uxcore: 3.2.0

## Support Devices

- Lenovo ThinkReality A3
- QONOQ MiRZA

## Biuld & Install

1. Download SnapDragon Spaces SDK from [Snapdragon Spaces Developer Portal](https://spaces.qualcomm.com/developer/)
1. Copy com.qualcomm.snapdragon.spaces-1.0.1.tgz to \Unity\DualRenderFusionMRTK3\Packages\SnapdragonSpaces
1. Open Unity Project
1. Set Project KeyStore
1. Build and create APK
1. Install APK in Snapdragon Spaces Devices

## How to use this app.

[See this page](HowToUse.md)

## License
This project is released under the MIT License.See the LICENSE file for more information.
