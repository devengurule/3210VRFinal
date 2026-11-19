using System;
using System.Collections.Generic;
using UnityEngine;

public enum CameraID
{
    registerCam,
    a1Cam,
    a2Cam,
    batteryCam
}

public class MonitorController : MonoBehaviour
{
    [SerializeField] private MeshRenderer monitor;
    [SerializeField] private Dictionary<CameraID, Material> cameraMaterial;

    private void Awake()
    {
        CameraButton.onPressed += MonitorButtonPressed;
    }

    private void MonitorButtonPressed(CameraID id)
    {
        monitor.material = cameraMaterial[id];
    }
}
