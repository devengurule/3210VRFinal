using UnityEngine;

public class RandomlyGenerateDecal : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private GameObject decalPrefab;
    [SerializeField] private GameObject decalPrefab2;

    private Vector3[] vertices;
    private Vector3[] normals;
    private Mesh mesh;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        normals = mesh.normals;

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
        Debug.Log(chosenVertex.ToString());

        GameObject decal = Instantiate(decalPrefab, transform);
        decal.transform.position = chosenVertex + chosenNormal.normalized * 0.1f;
        decal.transform.LookAt(chosenVertex);

        GameObject test = Instantiate(decalPrefab2, null);
        test.transform.position = chosenVertex;
    }
}
