using UnityEngine;
using TMPro;
using System.Collections;

public class WorldMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Timings")]
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float holdTime = 1.5f;
    [SerializeField] private float fadeOutTime = 0.5f;

    private Coroutine messageRoutine;

    void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowMessage(string message)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message)
    {
        messageText.text = message;

        // Fade in
        yield return Fade(0f, 1f, fadeInTime);

        // Hold
        yield return new WaitForSecondsRealtime(holdTime);

        // Fade out
        yield return Fade(1f, 0f, fadeOutTime);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
