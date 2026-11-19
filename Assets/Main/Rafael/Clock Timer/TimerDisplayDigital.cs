using TMPro;
using UnityEngine;

public class TimerDisplayDigital : MonoBehaviour
{
    private TextMeshProUGUI tmPro;

    private void OnEnable()
    {
        TimerScript.AnnounceTimeDigital += UpdateText;
    }

    private void OnDisable()
    {
        TimerScript.AnnounceTimeDigital -= UpdateText;
    }

    private void Start()
    {
        tmPro = GetComponent<TextMeshProUGUI>();
    }

    private void UpdateText(string text)
    {
        tmPro.text = text;
    }
}
