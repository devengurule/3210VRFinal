using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
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
    [SerializeField] private XROrigin xrorigin;
    [SerializeField] private MeshRenderer monitor;
    [SerializeField] private Dictionary<CameraID, Material> cameraMaterial;
    [SerializeField] private Dictionary<CameraID, Transform> teleportTransform;

    private CameraID currentCamera;
    private Transform currentPlayerTransform;

    private void Awake()
    {
        // Set the intial monitor to view the register camera
        monitor.material = cameraMaterial[CameraID.registerCam];
        currentCamera = CameraID.registerCam;
        currentPlayerTransform = teleportTransform[CameraID.registerCam];

        // Subscribe to the pressed event for the camera switch buttons
        CameraButton.onPressed += MonitorButtonPressed;
        ReturnButton.onPressed += ReturnButtonPressed;
    }

    private void OnDestroy()
    {
        // Unsubscribe
        CameraButton.onPressed -= MonitorButtonPressed;
        ReturnButton.onPressed -= ReturnButtonPressed;
    }

    private void MonitorButtonPressed(CameraID id)
    {
        // Switch the monitors material and store the current camera id
        monitor.material = cameraMaterial[id];
        currentCamera = id;
    }

    private void ReturnButtonPressed()
    {
        TeleportPlayer(teleportTransform[CameraID.registerCam]);
    }

    public void MoveButtonPressed()
    {
        TeleportPlayer(teleportTransform[currentCamera]);
    }

    private void TeleportPlayer(Transform position)
    {
        // Return if the players transform is equal to the selected transform
        if (currentPlayerTransform == position) return;

        // Store the players target transform
        currentPlayerTransform = position;

        // Get the target position from dictionary
        Vector3 targetPos = currentPlayerTransform.position;

        // Set y to players height
        targetPos.y += xrorigin.CameraInOriginSpaceHeight;

        // Move xr origin to target pos
        xrorigin.MoveCameraToWorldLocation(targetPos);
    }
}
