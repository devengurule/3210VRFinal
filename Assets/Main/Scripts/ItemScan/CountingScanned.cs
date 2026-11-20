using TMPro;
using UnityEngine;

public class CountingScanned : MonoBehaviour, I_ScanObject
{
    int currentValue = 0;
    int totalValue = 0;
    string currentName;
    TextMeshPro textObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textObject = GetComponent<TextMeshPro>();
    }


    public void ObjectScanned()
    {
        currentValue = 0;
    }
    public void SendingScannedValues(int iP, string iN)
    {
        currentValue = iP;
        totalValue += iP;
        currentName = iN;

        textObject.text = currentName + ": " + currentValue + " total: " + totalValue;
    }
}
