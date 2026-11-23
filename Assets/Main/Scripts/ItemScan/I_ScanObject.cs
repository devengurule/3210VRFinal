using UnityEngine;

// Interface for objects that can be scanned.
public interface I_ScanObject
{
    // this is called when the object is scanned.
    public void ObjectScanned();

    // this sends the scanned object's info.
    public void SendingScannedValues(int iP, string iN);

    // This sends the object's info when spawned, including its mesh.
    public void SendingSpawnedValues(int iP, string iN, Mesh iM);
}

public interface I_Items
{

    public void ItemsSpawned(int iAmount);
    public void AllItemsScanned();
}