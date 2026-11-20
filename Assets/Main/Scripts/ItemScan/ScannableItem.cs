using UnityEngine;

[CreateAssetMenu(fileName = "ScannableItem", menuName = "Scriptable Objects/ScannableItem")]
public class ScannableItem : ScriptableObject
{
    public string itemName;
    public int itemPrice;
    public Mesh itemMesh;
}
