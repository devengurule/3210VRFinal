using UnityEngine;

public class ShutterDistributionScript : MonoBehaviour
{
    [SerializeField] private Transform topOfFence;
    [SerializeField] private Transform bottomOfFence;

    private Transform[] bars;

    private void OnEnable()
    {
        LowerFencePart.FenceMoving += DistributeShutter;
    }

    private void OnDisable()
    {
        LowerFencePart.FenceMoving -= DistributeShutter;
    }

    private void Start()
    {
        bars = new Transform[transform.childCount];

        for (int i = 0; i < bars.Length; i++)
        {
            bars[i] = transform.GetChild(i);
        }

        DistributeShutter();
    }

    private void DistributeShutter()
    {
        Vector3 increment = (topOfFence.position - bottomOfFence.position) / (bars.Length);

        for (int i = 0; i < bars.Length; i++)
        {
            bars[i].localPosition = new Vector3(0, (increment.y * i), 0);
        }
    }
}
