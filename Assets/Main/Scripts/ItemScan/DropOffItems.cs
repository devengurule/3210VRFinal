using UnityEngine;

public class DropOffItems : MonoBehaviour
{

    [SerializeField] private Transform spawnLocation;
    [SerializeField] private GameObject spawnItem;
    [SerializeField] private ScannableItem[] scannableItems;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SpawnItem()
    {
        ScannableItem currentScanItem = scannableItems[Random.Range(0, scannableItems.Length)];
        GameObject spawnedItem = Instantiate(
            spawnItem,
            spawnLocation.position,
            Quaternion.identity
        );

        I_ScanObject m_AllScanables = spawnedItem.GetComponentInChildren<I_ScanObject>();
        m_AllScanables.SendingSpawnedValues(currentScanItem.itemPrice,currentScanItem.itemName,currentScanItem.itemMesh);
    }
}
