using UnityEngine;
using UnityEditor;

public class GenerateBiteMap
{
    [MenuItem("Tools/GenerateBiteMap", true)]
    public static bool SelectionValidation()
    {
        if (Selection.count == 0) return false;

        GameObject[] group = Selection.gameObjects;

        for (int i = 0; i < group.Length; i++)
        {
            if (!group[i].TryGetComponent<MeshFilter>(out MeshFilter m))
            {
                Debug.LogWarning("Missing MeshFilter on Selection");
                return false;
            }

            if (!group[i].TryGetComponent<Collider>(out Collider c))
            {
                Debug.LogWarning("Missing collider to consider on Selection");
                return false;
            }
        }

        return true;
    }

    [MenuItem("Tools/GenerateBiteMap", false)]
    public static void GenerateBiteMapAction()
    {
        GameObject[] group = Selection.gameObjects;

        for (int i = 0; i < group.Length; i++)
        {
            int validCount = ReturnValidCount(group[i]);

            int[] validIndeces = ReturnValidIndeces(group[i], validCount);

            //Create Scriptable Object(SO)

            //Use method to set SO's vertex array

            //Use method to set SO's normalArray

            //Add SO to Assets
        }
    }

    private static int ReturnValidCount(GameObject target)
    {
        int validCount = 0;
        bool valid = false;

        if (target.TryGetComponent<MeshFilter>(out MeshFilter m))
        {
            Collider[] c = target.GetComponents<Collider>();
            for (int i = 0; i < m.mesh.vertexCount; i++)
            {
                valid = true;
                for (int j = 0; j < c.Length; j++)
                {
                    if (c[j].bounds.Contains(m.mesh.vertices[i])) valid = false;
                }

                if (valid) validCount++;
            }
        }

        return validCount;
    }

    private static int[] ReturnValidIndeces(GameObject target, int size)
    {
        int[] validIndeces = new int[size];
        int index = 0;
        bool valid = false;

        if (target.TryGetComponent<MeshFilter>(out MeshFilter m))
        {
            Collider[] c = target.GetComponents<Collider>();
            for (int i = 0; i < m.mesh.vertexCount; i++)
            {
                valid = true;
                for (int j = 0; j < c.Length; j++)
                {
                    if (c[j].bounds.Contains(m.mesh.vertices[i])) valid = false;
                }

                if (valid)
                {
                    validIndeces[index] = i;
                    index++;
                }
            }
        }

        return validIndeces;
    }

    //private static Vector3[] ReturnValidGroup(Vector3[] arrayToWalk, int[] indeces) { }
}
