using System.Collections;
using UnityEngine;
using TMPro;

public class CharacterQuote : MonoBehaviour
{
    [Header("Dialogue")]
    public TMP_Text dialogueText;
    public GameObject dialogueBox;
    public string quote = "Courage first. Questions later.";

    [Header("Timing")]
    public float initialWait = 2f;
    public float displayTime = 2f;
    public float typeSpeed = 0.05f;

    private void Start()
    {
        StartCoroutine(QuoteLoop());
    }

    IEnumerator QuoteLoop()
    {
        while (true)
        {
            // Wait before showing the dialogue
            yield return new WaitForSeconds(initialWait);

            // Show the dialogue box
            dialogueBox.SetActive(true);

            // Type the quote
            yield return StartCoroutine(TypeLine(quote));

            // Keep the completed quote visible
            yield return new WaitForSeconds(displayTime);

            // Hide the dialogue box
            dialogueBox.SetActive(false);

            // Clear the text
            dialogueText.text = "";
        }
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
}