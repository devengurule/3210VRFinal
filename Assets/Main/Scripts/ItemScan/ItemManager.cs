using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour, I_Items, I_ScanObject
{
    private int spawnedItems = 0;
    private int scannedItems = 0;
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

    // keep empty
    public void ObjectScanned()
    {

    }

    // keep empty
    public void SendingSpawnedValues(int iP, string iN, Mesh iM)
    {
        
    }



    // keeping track of scanned values
    public void SendingScannedValues(int iP, string iN)
    {
        scannedItems += 1;
        Debug.Log(scannedItems);
        if (scannedItems >= spawnedItems)
        {
            for (int i = 0; i < m_AllTrackingSpawns.Count; i++)
            {
                m_AllTrackingSpawns[i].AllItemsScanned();
            }
            spawnedItems = 0;
            scannedItems = 0;

}

    }

    public void ItemsSpawned(int iAmount)
    {
        spawnedItems = iAmount;
        Debug.Log(spawnedItems);
    }
    // keep empty
    public void AllItemsScanned()
    {

    }

}
