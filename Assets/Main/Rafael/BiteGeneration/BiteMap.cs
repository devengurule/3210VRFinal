using UnityEngine;

[CreateAssetMenu(fileName = "BiteMap", menuName = "Scriptable Objects/BiteMap")]
public class BiteMap : ScriptableObject
{
    public Vector3[] MapVectices;
    public Vector3[] MapNormals;
}
