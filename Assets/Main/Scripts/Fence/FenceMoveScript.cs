using UnityEngine;

public class FenceMoveScript : MonoBehaviour
{
    public static event System.Action EventMove;

    [SerializeField] private Transform moveableAnchor;
    
    private bool isMoving = false;

    private Vector3 originPoint;
    private Vector3 highestPoint;

    private void Start()
    {
        originPoint = moveableAnchor.position;
        originPoint.y = 0.89f;
        highestPoint = originPoint;
        highestPoint.y = 1.7f;
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        try
        {
            EventMove.Invoke();
        }
        catch { }
    }

    private void LimitPos()
    {
        Vector3 pos = moveableAnchor.position; 
        pos.y = Mathf.Clamp(moveableAnchor.position.y, originPoint.y, highestPoint.y);
        moveableAnchor.position = pos;
    }

    public void SetMovement(bool setting)
    {
        isMoving = setting;
        LimitPos();
        EventMove.Invoke();
    }
}
