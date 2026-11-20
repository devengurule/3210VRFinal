using UnityEngine;

public interface I_ScanObject
{
    public void ObjectScanned();

    public void SendingScannedValues(int iP, string iN);
}