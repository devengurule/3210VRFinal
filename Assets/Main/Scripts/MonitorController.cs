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
    [SerializeField] private GameObject returnButton;
    [SerializeField] private Dictionary<CameraID, Material> cameraMaterial;
    [SerializeField] private Dictionary<CameraID, Transform> teleportTransform;

    private TransitionController transitioner;
    private CameraID currentCamera;
    private Transform currentPlayerTransform;

    private void Start()
    {
        // Disable the return button since the player starts at the register
        returnButton.SetActive(false);

        transitioner = GameController.instance.transitionController;

        // Set the intial monitor to view the register camera
        monitor.material = cameraMaterial[CameraID.registerCam];
        currentCamera = CameraID.registerCam;
        currentPlayerTransform = teleportTransform[CameraID.registerCam];

        // Subscribe to the pressed event for the camera switch buttons
        CameraButton.onPressed += MonitorButtonPressed;
        ReturnButton.onPressed += ReturnButtonPressed;
        transitioner.FadeEnded += TransitionedToBlack;
    }

    private void OnDestroy()
    {
        // Unsubscribe
        CameraButton.onPressed -= MonitorButtonPressed;
        ReturnButton.onPressed -= ReturnButtonPressed;
        transitioner.FadeEnded -= TransitionedToBlack;
    }

    private void MonitorButtonPressed(CameraID id)
    {
        // Switch the monitors material and store the current camera id
        monitor.material = cameraMaterial[id];
        currentCamera = id;
    }

    private void ReturnButtonPressed()
    {
        // Teleport back to the register
        TeleportPlayerSequence(teleportTransform[CameraID.registerCam]);
    }
    public void MoveButtonPressed()
    {
        // Teleport to where ever the current camera is looking at
        TeleportPlayerSequence(teleportTransform[currentCamera]);
    }

    private void TransitionedToBlack(float target)
    {
        // Only continue if the target alpha is 1(screen is targeting black)
        if (target != 1) return;

        // If the player is at the register turn off the return button
        // otherwise, turn on the button when they are anywhere else
        if (currentPlayerTransform == teleportTransform[CameraID.registerCam])
        {
            returnButton.SetActive(false);
        }
        else returnButton.SetActive(true);

        // Perform the teleport
        TeleportPlayer();
    }

    private void TeleportPlayerSequence(Transform position)
    {
        // Return if the players transform is equal to the selected transform
        if (currentPlayerTransform == position) return;

        // Store the players target transform
        currentPlayerTransform = position;

        // Fade to black
        transitioner.FadeToTargetAlpha(1);
    }

    private void TeleportPlayer()
    {
        // Get the target position from dictionary
        Vector3 targetPos = currentPlayerTransform.position;

        // Set y to players height
        targetPos.y += xrorigin.CameraInOriginSpaceHeight;

        // Move xr origin to target pos
        xrorigin.MoveCameraToWorldLocation(targetPos);

        // Fade back to reality, there goes gravity
        transitioner.FadeToTargetAlpha(0);
    }
}
