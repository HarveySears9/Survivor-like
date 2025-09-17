using System.Collections;
using UnityEngine;
using TMPro;

public class MenuDialogManager : MonoBehaviour
{
    [Header("Open Menu Lines")]
    public string[] greetingLines;

    [Header("Purchase Item Lines")]
    public string[] interactionLines;
    public string[] badInteractionLines;

    [Header("Settings")]
    public GameObject dialogueBubble;
    public float typeSpeed = 0.05f;
    public float startDelay = 0.5f;   // delay before showing the bubble
    public float hideDelay = 2f;      // delay after line finishes

    private TextMeshProUGUI dialogueText;
    private Coroutine typingCoroutine;

    private bool hide = true;

    void Awake()
    {
        // Get reference only once
        dialogueText = dialogueBubble.GetComponentInChildren<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        // Reset state
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        hide = true;

        dialogueText.text = "";
        dialogueBubble.SetActive(false);

        // Start the sequence
        typingCoroutine = StartCoroutine(ShowDialogue(greetingLines[Random.Range(0, greetingLines.Length)]));
    }

    IEnumerator ShowDialogue(string line)
    {
        if (hide)
        {
            yield return new WaitForSeconds(startDelay);
            hide = false;
        }

        dialogueBubble.SetActive(true);

        yield return StartCoroutine(TypeLine(line));

        // After typing finishes, wait then hide bubble
        yield return new WaitForSeconds(hideDelay);
        dialogueBubble.SetActive(false);
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    public void OnInteraction()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = "";
        dialogueBubble.SetActive(false);

        // Start the sequence
        typingCoroutine = StartCoroutine(ShowDialogue(interactionLines[Random.Range(0, interactionLines.Length)]));
    }

    public void OnBadInteraction()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = "";
        dialogueBubble.SetActive(false);

        // Start the sequence
        typingCoroutine = StartCoroutine(ShowDialogue(badInteractionLines[Random.Range(0, badInteractionLines.Length)]));
    }
}
