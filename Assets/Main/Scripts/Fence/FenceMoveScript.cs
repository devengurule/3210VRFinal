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

    private void LateUpdate()
    {
        if (!isMoving) return;

        LimitPos();

        float l = LimitPos();
        //if (l == originPoint.y || l == highestPoint.y) return;

        try
        {
            EventMove.Invoke();
        }
        catch { }
    }

    private float LimitPos()
    {
        Vector3 pos = moveableAnchor.position; 
        pos.y = Mathf.Clamp(moveableAnchor.position.y, originPoint.y, highestPoint.y);
        moveableAnchor.position = pos;

        return pos.y;
    }

    public void SetMovement(bool setting)
    {
        isMoving = setting;
        float l = LimitPos();
        EventMove.Invoke();
    }
}
