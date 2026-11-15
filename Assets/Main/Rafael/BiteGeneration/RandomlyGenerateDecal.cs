using UnityEngine;

public class RandomlyGenerateDecal : MonoBehaviour
{
    [Header("Highly Recommended")]
    [SerializeField] private BiteMap biteMap;
    [SerializeField] private GameObject skelPrefab;

    [Header("Required")]
    [SerializeField] private Mesh mesh;
    [SerializeField] private GameObject decalPrefab;

    [Header("Testing")]
    [SerializeField] private GameObject markPointPrefab;

    private Vector3[] vertices;
    private Vector3[] normals;

    private void Start()
    {
        if (mesh == null || decalPrefab == null)
        {
            Debug.LogError("REQUIRED VARIABLE MISSING IN " + gameObject.name);
            return;
        }

        if (biteMap == null)
        {
            vertices = mesh.vertices;
            normals = mesh.normals;
        }
        else
        {
            vertices = biteMap.MapVectices;
            normals = biteMap.MapNormals;
        }

        RandomizeChosenVertex();
        RandomizeChosenVertex();
        RandomizeChosenVertex();
        RandomizeChosenVertex();
        RandomizeChosenVertex();
    }

    private void RandomizeChosenVertex()
    {
        int rand = Random.Range(0, vertices.Length);
        Vector3 chosenVertex = vertices[rand];
        Vector3 chosenNormal = normals[rand];
        chosenVertex = gameObject.transform.TransformPoint(chosenVertex);
        //Debug.Log(chosenVertex.ToString());

        Transform t = gameObject.transform;
        if (skelPrefab != null) t = FindNearestBoneTransform(chosenVertex);

        GameObject decal = Instantiate(decalPrefab, t);
        decal.transform.position = chosenVertex + chosenNormal.normalized * 0.1f;
        decal.transform.LookAt(chosenVertex);

        //For testing purposes - marks the specific vertex that was generated
        if (markPointPrefab == null) return;
        GameObject test = Instantiate(markPointPrefab, t);
        test.transform.position = chosenVertex;
    }

    private Transform FindNearestBoneTransform(Vector3 vertex)
    {
        Transform closestBone = CheckBoneRecursive(skelPrefab.transform, null, float.PositiveInfinity, vertex);

        return closestBone;
    }

    private Transform CheckBoneRecursive(Transform boneToCheck, Transform closestBone, float closestDist, Vector3 point)
    {
        if (Vector3.Distance(boneToCheck.position, point) < closestDist)
        {
            closestBone = boneToCheck;
            closestDist = Vector3.Distance(closestBone.position, point);
        }

        for (int i = 0; i < boneToCheck.childCount; i++)
        {
            if (boneToCheck.GetChild(i).gameObject.GetComponents<Component>().Length > 1) continue;
            closestBone = CheckBoneRecursive(boneToCheck.GetChild(i), closestBone, closestDist, point);
            closestDist = Vector3.Distance(closestBone.position, point);
        }

        return closestBone;
    }
}
