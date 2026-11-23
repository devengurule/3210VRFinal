using TMPro;
using UnityEngine;

public class CountingScanned : MonoBehaviour, I_ScanObject, I_Items
{
    int currentValue = 0;
    int totalValue = 0;
    string currentName;


    TextMeshPro textObject;

    void Start()
    {
        // This gets the text component.
        textObject = GetComponent<TextMeshPro>();
    }

    public void ObjectScanned()
    {
        currentValue = 0;
    }

    // This is updating the scanned values and displays the total.
    public void SendingScannedValues(int iP, string iN)
    {
        currentValue = iP;
        totalValue += iP;
        currentName = iN;

        textObject.text = currentName + ": " + currentValue + " total: " + totalValue;
    }

    public void SendingSpawnedValues(int iP, string iN, Mesh iM)
    {

    }

    public void OrderCompleted()
    {
        currentValue = 0;
        totalValue = 0;
        currentName = "";
        textObject.text = "Order Finished";
    }

    // keep empty
    public void ItemsSpawned(int iAmount)
    {
        
    }

    public void AllItemsScanned()
    {
        OrderCompleted();
    }


}