using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Camera Stuff")]
    [SerializeField] private MeshRenderer cameraMonitor;
    [SerializeField] private GameObject returnButton;
    [SerializeField] private Dictionary<CameraID, Material> cameraMaterial;
    [SerializeField] private Dictionary<CameraID, Transform> teleportTransform;

    [Header("Shop Stuff")]
    [SerializeField] private MeshRenderer shopMonitor;
    [SerializeField] private GameObject bulletHighlight;
    [SerializeField] private GameObject batteryHighlight;
    [SerializeField] private TMP_Text bulletAmountPreview;
    [SerializeField] private TMP_Text bulletMaxAmountPreview;
    [SerializeField] private TMP_Text batteryAmountPreview;
    [SerializeField] private TMP_Text batteryMaxAmountPreview;
    [SerializeField] private TMP_Text totalAmountPreview;
    [SerializeField] private int MaxBattery;
    [SerializeField] private int MaxBullet;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject batteryPrefab;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private float spawnOffset;

    private enum CurrentItem
    {
        bullet,
        battery
    }

    private enum MonitorMode
    {
        camera,
        shop
    }

    private CurrentItem currentItem = CurrentItem.bullet;
    private int totalBatteryItems = 0;
    private int totalBulletItems = 0;
    private const int BULLET_PRICE = 100;
    private const int BATTERY_PRICE = 500;
    private TransitionController transitioner;
    private CameraID currentCamera;
    private MonitorMode mode;
    private Transform currentPlayerTransform;

    private void Start()
    {
        // Set inital values for the shop
        bulletMaxAmountPreview.text = MaxBullet.ToString();
        bulletAmountPreview.text = totalBulletItems.ToString();
        
        batteryMaxAmountPreview.text = MaxBattery.ToString();
        batteryAmountPreview.text = totalBatteryItems.ToString();

        totalAmountPreview.text = UpdateTotal().ToString();

        bulletHighlight.SetActive(currentItem == CurrentItem.bullet);
        batteryHighlight.SetActive(currentItem == CurrentItem.battery);

        mode = MonitorMode.shop;

        // Disable the return button since the player starts at the register
        returnButton.SetActive(false);

        transitioner = GameController.instance.transitionController;

        // Set the intial monitor to view the register camera
        cameraMonitor.material = cameraMaterial[CameraID.registerCam];
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
        cameraMonitor.material = cameraMaterial[id];
        currentCamera = id;
    }

    private void ReturnButtonPressed()
    {
        // Teleport back to the register
        TeleportPlayerSequence(teleportTransform[CameraID.registerCam]);
    }
    public void MoveButtonPressed()
    {
        if (mode != MonitorMode.camera) return;

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

    private int UpdateTotal()
    {
        totalAmountPreview.text = ((totalBatteryItems * BATTERY_PRICE) + (totalBulletItems * BULLET_PRICE)).ToString();

        return (totalBatteryItems * BATTERY_PRICE) + (totalBulletItems * BULLET_PRICE);
    }

    public void SwitchItem()
    {
        bulletHighlight.SetActive(currentItem == CurrentItem.battery);
        batteryHighlight.SetActive(currentItem == CurrentItem.bullet);

        currentItem = bulletHighlight.activeSelf ? CurrentItem.bullet : CurrentItem.battery;
    }

    public void AddItem()
    {
        if (currentItem == CurrentItem.battery)
        {
            if ((totalBatteryItems + 1) > MaxBattery) return;

            totalBatteryItems++;
            batteryAmountPreview.text = totalBatteryItems.ToString();
        }
        else
        {
            if ((totalBulletItems + 1) > MaxBullet) return;

            totalBulletItems++;
            bulletAmountPreview.text = totalBulletItems.ToString();
        }

        UpdateTotal();
    }

    public void RemoveItem()
    {
        if (currentItem == CurrentItem.battery)
        {
            if ((totalBatteryItems - 1) < 0) return;

            totalBatteryItems--;
            batteryAmountPreview.text = totalBatteryItems.ToString();
        }
        else
        {
            if ((totalBulletItems - 1) < 0) return;

            totalBulletItems--;
            bulletAmountPreview.text = totalBulletItems.ToString();
        }

        UpdateTotal();
    }

    public void ClearPurchase()
    {
        totalBatteryItems = 0;
        batteryAmountPreview.text = totalBatteryItems.ToString();

        totalBulletItems = 0;
        bulletAmountPreview.text = totalBulletItems.ToString();

        UpdateTotal();
    }

    public void CompletePurchase()
    {
        if (mode != MonitorMode.shop) return;

        StartCoroutine(SpawnItems());
    }

    private IEnumerator SpawnItems()
    {
        // Spawn Batteries
        for (int i = 0; i < totalBatteryItems; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-spawnOffset, spawnOffset), 0f, Random.Range(-spawnOffset, spawnOffset));

            Instantiate(batteryPrefab, spawnPosition.position + randomOffset, Quaternion.identity);

            yield return null;
        }

        // Spawn Bullets
        for (int i = 0; i < totalBulletItems; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-spawnOffset, spawnOffset), 0f, Random.Range(-spawnOffset, spawnOffset));
            
            Instantiate(bulletPrefab, spawnPosition.position + randomOffset, Quaternion.identity);

            yield return null;
        }

        ClearPurchase();
    }
}
