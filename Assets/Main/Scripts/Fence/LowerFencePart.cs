using System;
using UnityEngine;

public class LowerFencePart : MonoBehaviour
{
    public static event Action FenceMoving;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.y == 0) return;

        FenceMoving?.Invoke();
    }
}
