using System.Collections.Generic;
using UnityEngine;

public class DropOffItems : MonoBehaviour
{

    [SerializeField] private Transform spawnLocation;
    [SerializeField] private GameObject spawnItem;
    [SerializeField] private ScannableItem[] scannableItems;
    [SerializeField] private int howManyItems;
    private List<I_Items> m_AllTrackingSpawns = new List<I_Items>();
    void Start()
    {
        // This finds all objects that use the scan interface.
        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();

        for (int i = 0; i < allScripts.Length; i++)
        {
            if (allScripts[i] is I_Items && allScripts[i] != this)
                m_AllTrackingSpawns.Add(allScripts[i] as I_Items);
        }
    }

    public void SpawnItem()
    {
        for (int i = 0; i < howManyItems; i++)
        {
            ScannableItem currentScanItem = scannableItems[Random.Range(0, scannableItems.Length)];
            GameObject spawnedItem = Instantiate(
                spawnItem,
                spawnLocation.position,
                Quaternion.identity
            );

            I_ScanObject m_AllScanables = spawnedItem.GetComponentInChildren<I_ScanObject>();
            m_AllScanables.SendingSpawnedValues(currentScanItem.itemPrice, currentScanItem.itemName, currentScanItem.itemMesh);
        }

        for (int i = 0; i < m_AllTrackingSpawns.Count; i++)
        {
            m_AllTrackingSpawns[i].ItemsSpawned(howManyItems);
        }
    }
}
