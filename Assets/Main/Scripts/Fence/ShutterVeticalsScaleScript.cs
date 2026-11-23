using UnityEngine;

public class ShutterVeticalsScaleScript : MonoBehaviour
{
    [SerializeField] private Transform topAnchor;

    private float distanceForScale1;
    private bool scale = false;

    private void Start()
    {
        distanceForScale1 = (topAnchor.position.y - transform.position.y) / transform.localScale.y;
    }

    private void FixedUpdate()
    {
        if (scale) ScaleVerticals();
    }

    private void ScaleVerticals()
    {
        transform.localScale = new Vector3(1, (topAnchor.position.y - transform.position.y) / distanceForScale1, 1);
    }

    public void SetScale(bool setting)
    {
        scale = setting;
    }
}
