using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    private TextMeshProUGUI tmPro;

    private void OnEnable()
    {
        TimerScript.AnnounceTime += UpdateText;
    }

    private void OnDisable()
    {
        TimerScript.AnnounceTime -= UpdateText;
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
