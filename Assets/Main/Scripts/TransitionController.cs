using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private Material blackMaterial;
    [SerializeField] private float fadeDuration;
    private Coroutine FadeRoutine;
    public event Action<float> FadeEnded;

    private void Awake()
    {
        // Make sure the alpha is 0 so the player can see
        blackMaterial.SetFloat("_Alpha", 0f);
    }

    public void FadeToTargetAlpha(float target)
    {
        // Stop the coroutine if one is running
        if (FadeRoutine != null)
        {
            StopCoroutine(FadeRoutine);
        }

        // Start fade coroutine
        FadeRoutine = StartCoroutine(FadeEnumerator(target));
    }

    private IEnumerator FadeEnumerator(float target)
    {
        float duration = fadeDuration;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Lerp between the current alpha and the target alpha, t is the percetage of change
            float newAlpha = Mathf.Lerp(blackMaterial.GetFloat("_Alpha"), target, t);

            // Set the new alpha
            blackMaterial.SetFloat("_Alpha", newAlpha);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Set the alpha to target to make sure it is actually set to target
        blackMaterial.SetFloat("_Alpha", target);

        FadeEnded?.Invoke(target);
    }
}
