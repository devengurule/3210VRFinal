using System.Collections.Generic;
using UnityEngine;

public class ItemType : MonoBehaviour, I_ScanObject
{
    private string itemName;
    private int itemPrice;

    private List<I_ScanObject> m_AllScannables = new List<I_ScanObject>();

    [SerializeField] private MeshCollider objectCollider;
    [SerializeField] private MeshFilter objectFilter;

    void Start()
    {
        // This finds all objects that use the scan interface.
        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();

        for (int i = 0; i < allScripts.Length; i++)
        {
            if (allScripts[i] is I_ScanObject && allScripts[i] != this)
                m_AllScannables.Add(allScripts[i] as I_ScanObject);
        }
    }

    // This sends this item's info to all registered scanners.
    public void ObjectScanned()
    {
        for (int i = 0; i < m_AllScannables.Count; i++)
        {
            m_AllScannables[i].SendingScannedValues(itemPrice, itemName);
        }
    }

    public void SendingScannedValues(int iP, string iN)
    {

    }

    // This updates the item's info and mesh when spawned.
    public void SendingSpawnedValues(int iP, string iN, Mesh iM)
    {
        Debug.Log("stuff scanned");

        itemName = iN;
        itemPrice = iP;

        objectCollider.sharedMesh = iM;
        objectFilter.mesh = iM;
    }
}