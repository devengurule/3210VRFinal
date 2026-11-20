using System;
using UnityEngine;

public class ReturnButton : MonoBehaviour
{
    public static event Action onPressed;

    public void PressedButton()
    {
        onPressed?.Invoke();
    }
}
