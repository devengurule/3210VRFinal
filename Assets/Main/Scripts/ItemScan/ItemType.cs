using System.Collections.Generic;
using UnityEngine;

public class ItemType : MonoBehaviour,I_ScanObject
{
    public string itemName;
    public int itemPrice;
    List<I_ScanObject> m_AllScannables = new List<I_ScanObject>();
    void Start()
    {
        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();
        for (int i = 0; i < allScripts.Length; i++)
        {
            if (allScripts[i] is I_ScanObject)
                m_AllScannables.Add(allScripts[i] as I_ScanObject);
        }
    }



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
}
