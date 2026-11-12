using UnityEngine;

public class RandomlyGenerateDecal : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private GameObject decalPrefab;

    private Vector3[] vertices;
    private Mesh mesh;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        RandomizeChosenVertex();
        RandomizeChosenVertex();
        RandomizeChosenVertex();
        RandomizeChosenVertex();
        RandomizeChosenVertex();
    }

    private void RandomizeChosenVertex()
    {
        Vector3 chosen = vertices[Random.Range(0, vertices.Length)];
        chosen = gameObject.transform.TransformPoint(chosen);
        Debug.Log(chosen.ToString());

        GameObject decal = Instantiate(decalPrefab, null);
        decal.transform.position = chosen;
    }
}
