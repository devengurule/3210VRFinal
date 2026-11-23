using UnityEngine;

public class FenceMoveScript : MonoBehaviour
{
    public static event System.Action EventMove;
    public static event System.Action ShutterLowered;
    public static event System.Action ShutterRaised;

    [SerializeField] private Transform moveableAnchor;
    
    private bool isMoving = false;

    private Vector3 lowestPoint;
    private Vector3 highestPoint;
    private bool isLowered = true;

    private readonly float LOWEST_Y = 0.89f;
    private readonly float HIGHEST_Y = 2.2f;

    private void Start()
    {
        lowestPoint = moveableAnchor.position;
        lowestPoint.y = LOWEST_Y;
        highestPoint = lowestPoint;
        highestPoint.y = HIGHEST_Y;
    }

    private void LateUpdate()
    {

        if (!isMoving) moveableAnchor.position += new Vector3(0, 0.1f * Mathf.Sign(-0.5f * System.Convert.ToInt32(isLowered)), 0);

        LimitPos();
        try
        {
            EventMove.Invoke();
        }
        catch { }
    }

    private float LimitPos()
    {
        Vector3 pos = moveableAnchor.position; 
        pos.y = Mathf.Clamp(moveableAnchor.position.y, lowestPoint.y, highestPoint.y);
        moveableAnchor.position = pos;

        return pos.y;
    }

    private void SetStateBool()
    {
        isLowered = true;
        if (moveableAnchor.position.y > ((HIGHEST_Y - LOWEST_Y) * 0.6f) + LOWEST_Y) isLowered = false;
        AnnounceShutterState();
    }

    private void AnnounceShutterState()
    {
        try
        {
            if (isLowered) ShutterLowered.Invoke();
            if (!isLowered) ShutterRaised.Invoke();
        }
        catch { }
    }

    public void SetMovement(bool setting)
    {
        isMoving = setting;
        float l = LimitPos();
        EventMove.Invoke();
        SetStateBool();
    }
}
