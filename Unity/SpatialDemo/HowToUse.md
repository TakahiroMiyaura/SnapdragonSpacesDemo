# How To Use
[README.md](/README.md)

## Basic Operations

1. Start the Application<BR>At first startup, nothing is displayed on the AR Glass. This is because Auto Start On Display Connect is disabled in the Dynamic OpenXR Loader function. If you want to start it automatically from the next time onward, please change it from the setting menu.
1. Use as a smartphone application<BR>When AR glasses are not connected, it works as a smartphone application.The character can be controlled with the controller on the touch screen.
1. Use as AR application<BR>Connect AR glasses to your smartphone. After the connection is complete, press the Start button on the Dynamic OpenXR Loader. Three buttons will appear in the center of the screen.Please follow the steps below.Once the operation is completed, you can use it as an AR application.
    1. Tap 'Turn on Camera Access'
    1. Tap 'Turn on Spatial Mapping'
    1. Tap 'Trurn off Plane & Mesh Visiualization'
1. Synchronize Transform between smartphone and AR glasses<BR>Tap 'Sync with AR Camera' to switch between the following.
    1. Smartphone screen view changes in accordance with the Transform of AR glasses.
    1. The virtual stick on the right controls Transform on the smartphone.

## Smart Phone Touch Screen

![](/images/TouchScreen.png)

1. Character manipulation (Analog Stick)
1. Camera Angle manipulation (Analog Stick)
1. Jump (Button)
1. Settings (Button)

## Spatial Meshing

This app uses AR Foundation for spatial recognition; Collision is enabled for AR glasses and Mesh is hidden.　On the other hand, the smartphone screen visualizes the spatially recognized area.

## Settings

You can change various setting in menu. The changed settings are saved and persisted in the application.

### OpenXR Settings

![](/images/Settings1.png)
You can change the settings of Dynamic OpenXR Loader.

- Auto Start On Display Connect
- Auto Manage XR Camera
- if Auto Start On Display Connect is false...
  - Tap Start Button, OpenXR start to load.
  - Tap Stop Button, OpenXR shutdown. 


### Hand Tracking Settings

![](/images/Settings2.png)
You can change the settings of Hand Tracking.
- Left Hand
- Right Hand

Setting disable, Hand tracking is disabled. If Hand Tracking is enabled,you can manipulate the caracter  using hands.


### Debug Info Visualize Settings

![](/images/Settings3.png)
The status of the glass and the touchscreen controller can be displayed.To hide the status, set the corresponding button to Off.
