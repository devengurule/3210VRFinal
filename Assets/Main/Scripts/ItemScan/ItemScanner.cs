using System.Collections.Generic;
using UnityEngine;

public class ItemScanner : MonoBehaviour
{

    [Header("Detection Settings")]
    [SerializeField] private string hitObject;
    public Transform target;

    [SerializeField] private LayerMask layerMask = Physics.DefaultRaycastLayers;
    public QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore;

    public void ScanObject()
    {
        if (target == null)
            return;

        if (Physics.Linecast(
       transform.position,
       target.position,
       out RaycastHit hit,
       layerMask,
       queryTriggerInteraction))
        {
            Debug.Log("scanned");
            if (string.IsNullOrEmpty(hitObject) ||
                     hit.collider.gameObject.tag != hitObject)
                return;

            I_ScanObject m_AllScanables = hit.collider.gameObject.GetComponentInChildren<I_ScanObject>();
            m_AllScanables.ObjectScanned();

        }
    }
}
