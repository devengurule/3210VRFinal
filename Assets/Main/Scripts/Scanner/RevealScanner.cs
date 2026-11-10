
using UnityEngine;

public class RevealScanner : MonoBehaviour
{
    [Header("Scanner Settings")]
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private float softness = 0.25f;

    [Header("Detection Settings")]
    [SerializeField] private string hitObject;
    public Transform target;

    [SerializeField] private LayerMask layerMask = Physics.DefaultRaycastLayers;
    public QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore;

    private Renderer activeRenderer;

    private MaterialPropertyBlock scannerBlock;

    private static readonly int scannerPositionProperty =
        Shader.PropertyToID("_ScannerPosition");

    private static readonly int radiusProperty =
        Shader.PropertyToID("_Radius");

    private static readonly int softnessProperty =
        Shader.PropertyToID("_Softness");

    private void Awake()
    {
        scannerBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        DetectingObject();
    }

    private void DetectingObject()
    {
        if (target == null)
        {
            ClearPreviousRenderer();
            return;
        }

        if (Physics.Linecast(
            transform.position,
            target.position,
            out RaycastHit hit,
            layerMask,
            queryTriggerInteraction))
        {
            if (!string.IsNullOrEmpty(hitObject) &&
                hit.collider.gameObject.tag != hitObject)
            {
                ClearPreviousRenderer();
                return;
            }

            Renderer detectedRenderer =
                hit.collider.GetComponent<Renderer>();

            if (detectedRenderer == null)
            {
                detectedRenderer =
                    hit.collider.GetComponentInParent<Renderer>();
            }

            if (detectedRenderer == null)
            {
                ClearPreviousRenderer();
                return;
            }

            activeRenderer = detectedRenderer;

            UsingScanner();
        }
        else
        {
            ClearPreviousRenderer();
        }
    }

    private void UsingScanner()
    {
        if (activeRenderer == null)
            return;

        // Get the existing property block.
        activeRenderer.GetPropertyBlock(scannerBlock);

        // Send scanner position to the shader.
        scannerBlock.SetVector(
            scannerPositionProperty,
            transform.position
        );

        // Send scanner radius.
        scannerBlock.SetFloat(
            radiusProperty,
            Mathf.Max(0f, radius)
        );

        // Send transition softness.
        scannerBlock.SetFloat(
            softnessProperty,
            Mathf.Max(0.001f, softness)
        );

        // Apply the updated properties.
        activeRenderer.SetPropertyBlock(scannerBlock);
    }

    private void ClearPreviousRenderer()
    {
        if (activeRenderer == null)
            return;

        // Restore visibility by setting the radius to zero.
        activeRenderer.GetPropertyBlock(scannerBlock);
        scannerBlock.SetFloat(radiusProperty, 0f);
        activeRenderer.SetPropertyBlock(scannerBlock);

        activeRenderer = null;
    }

    private void OnDisable()
    {
        ClearPreviousRenderer();
    }
}
