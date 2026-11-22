using UnityEngine;

public class FenceMoveScript : MonoBehaviour
{
    [SerializeField] private Transform topAnchor;
    [SerializeField] private Transform bottomAnchor;
    [SerializeField] private Transform floorAnchor;
    [SerializeField] private float speed;

    [SerializeField] private float topOffset;
    [SerializeField] private float bottomOffset;
    private bool isRaising = false;
    private bool isLowering = false;
    private Vector3 modifiablePos;

    private void Start()
    {
        modifiablePos = transform.position;
        topOffset = bottomAnchor.position.y - topAnchor.position.y;
        bottomOffset = bottomAnchor.position.y - floorAnchor.position.y;
    }

    private void FixedUpdate()
    {
        if (!isRaising || !isLowering) return;

        if (isLowering) modifiablePos.y = Mathf.Clamp(modifiablePos.y - (speed * Time.deltaTime), topOffset, bottomOffset);
        if (isRaising) modifiablePos.y = Mathf.Clamp(modifiablePos.y + (speed * Time.deltaTime), topOffset, bottomOffset);
    }

    private void StopMovement()
    {
        isRaising = false;
        isLowering = false;
    }

    private void RaiseFence()
    {
        StopMovement();
        isRaising = true;
    }

    private void DropFence()
    {
        StopMovement();
        isLowering = true;
    }
}
