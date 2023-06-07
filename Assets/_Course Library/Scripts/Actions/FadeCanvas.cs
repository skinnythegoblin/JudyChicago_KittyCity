using System.Collections;
using UnityEngine;

/// <summary>
/// Fades a canvas over time using a coroutine and a canvas group
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class FadeCanvas : MonoBehaviour
{
    [Tooltip("The speed at which the canvas fades")]
    public float defaultDuration = 1.0f;

    public Coroutine CurrentRoutine { private set; get; } = null;

    private CanvasGroup canvasGroup = null;
    private float alpha = 0.0f;

    private float quickFadeDuration = 0.25f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void StartFadeIn()
    {
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(FadeIn(defaultDuration));
    }

    public void StartFadeOut()
    {
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(FadeOut(defaultDuration));
    }

    public void StartBounceIn()
    {
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(FadeBounceIn(defaultDuration));
    }
    
    public void StartBounceOut()
    {
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(FadeBounceOut(defaultDuration));
    }

    public void QuickFadeIn()
    {
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(FadeIn(quickFadeDuration));
    }

    public void QuickFadeOut()
    {
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(FadeOut(quickFadeDuration));
    }

    public void QuickBounceIn()
    {
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(FadeBounceIn(quickFadeDuration));
    }

    public void QuickBounceOut()
    {
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(FadeBounceOut(quickFadeDuration));
    }

    private IEnumerator FadeIn(float duration)
    {
        float elapsedTime = 0.0f;

        while (alpha <= 1.0f)
        {
            SetAlpha(elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOut(float duration)
    {
        float elapsedTime = 0.0f;

        while (alpha >= 0.0f)
        {
            SetAlpha(1 - (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeBounceIn(float duration)
    {
        CurrentRoutine = StartCoroutine(FadeOut(duration));
        yield return new WaitForSeconds(duration * 2);
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(FadeIn(duration));
    }

    private IEnumerator FadeBounceOut(float duration)
    {
        CurrentRoutine = StartCoroutine(FadeIn(duration));
        yield return new WaitForSeconds(duration * 2);
        StopAllCoroutines();
        CurrentRoutine = StartCoroutine(FadeOut(duration));
    }

    private void SetAlpha(float value)
    {
        alpha = value;
        canvasGroup.alpha = alpha;
    }
}