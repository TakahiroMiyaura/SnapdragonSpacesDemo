using System.Collections;
using System.Collections.Generic;
using MixedReality.Toolkit.SpatialManipulation;
using Reseul.Snapdragon.Spaces.Devices;
using UnityEngine;

public class DialogModeSwitcher : MonoBehaviour
{

    public SolverHandler solverHandler;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var type = DeviceConfirmProvider.GetCurrentDeviceType();

        switch (type)
        {
            case XRDeviceType.ThinkRealityVRX:
                canvas.renderMode = RenderMode.WorldSpace;
                if (gameObject.GetComponent<Follow>() == null)
                {
                    Follow followSolver = gameObject.AddComponent<Follow>();
                    followSolver.Smoothing = true;
                    followSolver.MoveLerpTime = 1.0f;
                    followSolver.RotateLerpTime = 1.0f;
                    followSolver.OrientToControllerDeadZoneDegrees = 25.0f;
                }
                solverHandler.UpdateSolvers = true;
                break;
            case XRDeviceType.ThinkRealityA3:
                solverHandler.UpdateSolvers = false;
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                break;
            case XRDeviceType.Unknown:
                break;
        }
    }
}
