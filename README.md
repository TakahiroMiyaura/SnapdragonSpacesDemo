# Snapdragon Spaces Platform Demo Application

This is a sample application that allows you to verify the functionality of Snapdragon Spaces.

* MiRAZA Library Demo
* Spatial Demo


## MiRZA Library Demo

[![Demo Video](https://img.youtube.com/vi/9_BkXJyxl3E/0.jpg)](https://youtu.be/9_BkXJyxl3E)

### Summary

This sample is for verifying the functions of the MiRZA Plugin.
This sample uses Dual Render Fusion. On a smartphone screen, you can use the MiRZA Plugin to check various types of information.
When you perform the following operations using the touch sensor, the ring made up of spheres will move.

* HostView(Smart Phone) features 
    * MiRZA Library Monitoring ON/OFF
    * Get Current Status of MiRZA Device Info
    * Spaces Mode ON/OFF
    * Mic Mode Change & Recording, Play
* AR Glass feature
    * When you perform a "move" gesture on the MiRZA touch sensor, the ring will rotate.


![](/images/TouchScreen.png)

1. Monitoring On/Off (Toggle) ※ you must set "on" first!
2. Launch MiRZA App Button
3. Change view Camera Frame Access mode.
4. Event Handler enable/disable (Toggle)
5. Get Current Status of MiRZA device.
6. When MiRZA device status changed, it display the status info.
7. Spaces Mode ON/OFF (Toggle)
8. Mic Mode Setting & Recording,Play
    * The three buttons on the right are arranged in the order of record, stop, and play.
    * after Mic Mode setting,tap record button,it start recording using MiRZA devide microphone.
    * After tap record button, when tap stop button, it finish recording.
    * Last, tap play button, it play the recording Audio

### Develop Environments

- Unity 2022.3.36f1
- Snapdragon Spaces SDk V1.0.1
- Mixed Reality Toolkit 3
   - org.mixedrealitytoolkit.core: 3.2.2
   - org.mixedrealitytoolkit.input: 3.2.2
   - org.mixedrealitytoolkit.spatialmanipulation: 3.3.1
   - org.mixedrealitytoolkit.standardassets: 3.2.0
   - org.mixedrealitytoolkit.uxcomponents: 3.3.0
   - org.mixedrealitytoolkit.uxcore: 3.2.2
- MRTK Graphics Tools 0.8.0

### Support Devices

- QONOQ MiRZA

### Build & Install

1. Download SnapDragon Spaces SDK from [Snapdragon Spaces Developer Portal](https://spaces.qualcomm.com/developer/)
1. Copy com.qualcomm.snapdragon.spaces-1.0.1.tgz to \Unity\SpatialDemo\Packages\SnapdragonSpaces
1. Set up MRTK3 to this Unity project from UPM.
    - Note : We can also set up it using [Mixed Reality Feature Tool](https://learn.microsoft.com/ja-jp/windows/mixed-reality/develop/unity/welcome-to-mr-feature-tool?wt.mc_id=WDIT-MVP-5003104)[^1].
    - Required Components:
        - MRTK Graphics Tools
        - MRTK Core Definitions
        - MRTK Input
        - MRTK Spatial Mnipulation
        - MRTK Standard Assets
        - MRTK UX Components
        - MRTK UX Core Scripts
1. Open Unity Project and setting
    1. Switch Platform to Android
    1. Open Scene [Reseul\Scenes\SampleScene]
    1. Import TextMeshPro Essential Resources
    1. Open [Project Settings\Player Settings\Publishing Settings] and set Project KeyStore
1. Build and create APK
1. Install APK in Snapdragon Spaces Devices

[^1]: See also [MRTK3 Setup Guide(in Snapdragon Spaces Platform web sites)](https://docs.spaces.qualcomm.com/unity/samples/preview/mrtk3-setup-guide) 

## Spatial Demo

[![Demo Video](https://img.youtube.com/vi/dyjCIETCc6c/0.jpg)](https://www.youtube.com/watch?v=dyjCIETCc6c)

### Summary
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
    - [MRTK Graphics Tools](https://github.com/microsoft/MixedReality-GraphicsTools-Unity)

### Develop Environments

- Unity 2022.3.36f1
- Snapdragon Spaces SDk V1.0.1
- Mixed Reality Toolkit 3
   - org.mixedrealitytoolkit.core: 3.2.2
   - org.mixedrealitytoolkit.input: 3.2.2
   - org.mixedrealitytoolkit.spatialmanipulation: 3.3.1
   - org.mixedrealitytoolkit.standardassets: 3.2.0
   - org.mixedrealitytoolkit.uxcomponents: 3.3.0
   - org.mixedrealitytoolkit.uxcore: 3.2.2
- MRTK Graphics Tools 0.8.0

### Support Devices

- Lenovo ThinkReality A3
- QONOQ MiRZA

### Build & Install

1. Download SnapDragon Spaces SDK from [Snapdragon Spaces Developer Portal](https://spaces.qualcomm.com/developer/)
1. Copy com.qualcomm.snapdragon.spaces-1.0.1.tgz to \Unity\SpatialDemo\Packages\SnapdragonSpaces
1. Set up MRTK3 to this Unity project from UPM.
    - Note : We can also set up it using [Mixed Reality Feature Tool](https://learn.microsoft.com/ja-jp/windows/mixed-reality/develop/unity/welcome-to-mr-feature-tool?wt.mc_id=WDIT-MVP-5003104)[^1].
    - Required Components:
        - MRTK Graphics Tools
        - MRTK Core Definitions
        - MRTK Input
        - MRTK Spatial Mnipulation
        - MRTK Standard Assets
        - MRTK UX Components
        - MRTK UX Core Scripts
1. Open Unity Project and setting
    1. Switch Platform to Android
    1. Open Scene [Reseul\Scenes\SampleScene]
    1. Import TextMeshPro Essential Resources
    1. Open [Project Settings\Player Settings\Publishing Settings] and set Project KeyStore
1. Build and create APK
1. Install APK in Snapdragon Spaces Devices

[^1]: See also [MRTK3 Setup Guide(in Snapdragon Spaces Platform web sites)](https://docs.spaces.qualcomm.com/unity/samples/preview/mrtk3-setup-guide) 

### How to use this app.

[See this page](Unity/SpatialDemo/HowToUse.md)

## License
This project is released under the MIT License.See the LICENSE file for more information.
