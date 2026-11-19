using System;
using UnityEngine;

public class CameraButton : MonoBehaviour
{
    [SerializeField] private CameraID camID;

    public static event Action<CameraID> onPressed;

    public void PressedButton()
    {
        onPressed?.Invoke(camID);
    }
}
