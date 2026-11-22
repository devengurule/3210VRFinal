using UnityEngine;

public class FenceMoveScript : MonoBehaviour
{
    public static event System.Action EventMove;

    [SerializeField] private Transform moveableAnchor;
    
    private bool isMoving = false;
    private void FixedUpdate()
    {
        if (!isMoving) return;

        try
        {
            EventMove.Invoke();
        }
        catch { }
    }

    public void SetMovement(bool setting)
    {
        isMoving = setting;
    }
}
