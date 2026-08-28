using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    // ============================================================
    // CUTSCENE DATA
    // ============================================================

    [Header("Cutscene")]

    public CutsceneData cutscene;


    // ============================================================
    // SCENE REFERENCES
    // ============================================================

    [Header("Background")]

    public GameObject backgroundHolder;


    [Header("Characters")]

    public Transform character1Position;
    public Transform character2Position;


    // ============================================================
    // SPEECH BUBBLES
    // ============================================================

    [Header("Speech Bubbles")]

    public GameObject character1Bubble;
    public GameObject character2Bubble;

    public TextMeshProUGUI character1Text;
    public TextMeshProUGUI character2Text;


    // ============================================================
    // SETTINGS
    // ============================================================

    [Header("Typing Settings")]

    public float typeSpeed = 0.05f;


    // ============================================================
    // PRIVATE VARIABLES
    // ============================================================

    private int currentLineIndex = 0;

    private Coroutine typingCoroutine;

    private bool isTyping = false;

    private string currentFullLine;


    private GameObject currentCharacter1;
    private GameObject currentCharacter2;
    private GameObject currentBackground;


    // ============================================================
    // START
    // ============================================================

    void Start()
    {
        cutscene = CutsceneLoader.Instance.GetCutscene();

        if (cutscene == null)
        {
            Debug.LogError(
                "CutsceneManager: No CutsceneData was provided!"
            );

            return;
        }

        PlayCutscene(cutscene);
    }


    // ============================================================
    // PLAY CUTSCENE
    // ============================================================

    public void PlayCutscene(CutsceneData data)
    {
        if (data == null)
        {
            Debug.LogWarning(
                "Tried to play a null CutsceneData."
            );

            return;
        }


        // Store the cutscene
        cutscene = data;


        // Reset dialogue
        currentLineIndex = 0;


        // Clear any previous objects
        ClearCutsceneObjects();


        // Create the background
        if (cutscene.background != null)
        {
            currentBackground =
                Instantiate(
                    cutscene.background,
                    backgroundHolder.transform
                );
        }


        // Create Character 1
        if (cutscene.character1 != null)
        {
            currentCharacter1 =
                Instantiate(
                    cutscene.character1,
                    character1Position
                );

            currentCharacter1.transform.localPosition =
                Vector3.zero;

            currentCharacter1.transform.localRotation =
                Quaternion.identity;
        }


        // Create Character 2
        if (cutscene.character2 != null)
        {
            currentCharacter2 =
                Instantiate(
                    cutscene.character2,
                    character2Position
                );

            currentCharacter2.transform.localPosition =
                Vector3.zero;

            currentCharacter2.transform.localRotation =
                Quaternion.identity;

            // Flip Character 2 horizontally
            Vector3 scale = currentCharacter2.transform.localScale;
            scale.x *= -1f;
            currentCharacter2.transform.localScale = scale;
        }


        // Hide speech bubbles
        character1Bubble.SetActive(false);
        character2Bubble.SetActive(false);


        // Check dialogue
        if (
            cutscene.dialogueLines == null ||
            cutscene.dialogueLines.Length == 0
        )
        {
            Debug.LogWarning(
                "Cutscene has no dialogue lines."
            );

            return;
        }


        // Start first line
        ShowCurrentLine();
    }


    // ============================================================
    // CLEAR PREVIOUS CUTSCENE
    // ============================================================

    void ClearCutsceneObjects()
    {
        if (currentCharacter1 != null)
        {
            Destroy(currentCharacter1);
        }

        if (currentCharacter2 != null)
        {
            Destroy(currentCharacter2);
        }

        if (currentBackground != null)
        {
            Destroy(currentBackground);
        }
    }


    // ============================================================
    // NEXT BUTTON
    // ============================================================

    public void NextLine()
    {
        // If currently typing,
        // finish the line instead of skipping it.
        if (isTyping)
        {
            FinishTyping();

            return;
        }


        // Move to next line
        currentLineIndex++;


        // Check if cutscene is finished
        if (
            currentLineIndex >=
            cutscene.dialogueLines.Length
        )
        {
            EndCutscene();

            return;
        }


        // Show next line
        ShowCurrentLine();
    }


    // ============================================================
    // SHOW CURRENT LINE
    // ============================================================

    void ShowCurrentLine()
    {
        CutsceneData.CutsceneLine currentLine =
            cutscene.dialogueLines[currentLineIndex];


        // Hide both bubbles
        character1Bubble.SetActive(false);
        character2Bubble.SetActive(false);


        // Stop previous typing
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }


        // Character 1 speaking
        if (currentLine.speaker == 0)
        {
            character1Bubble.SetActive(true);

            currentFullLine =
                currentLine.line;

            typingCoroutine =
                StartCoroutine(
                    TypeLine(
                        currentFullLine,
                        character1Text
                    )
                );
        }


        // Character 2 speaking
        else if (currentLine.speaker == 1)
        {
            character2Bubble.SetActive(true);

            currentFullLine =
                currentLine.line;

            typingCoroutine =
                StartCoroutine(
                    TypeLine(
                        currentFullLine,
                        character2Text
                    )
                );
        }


        else
        {
            Debug.LogWarning(
                "Invalid speaker index: " +
                currentLine.speaker
            );
        }
    }


    // ============================================================
    // TYPE LINE
    // ============================================================

    IEnumerator TypeLine(
        string line,
        TextMeshProUGUI textObject
    )
    {
        isTyping = true;

        textObject.text = "";


        foreach (char c in line.ToCharArray())
        {
            textObject.text += c;

            yield return new WaitForSeconds(typeSpeed);
        }


        isTyping = false;

        typingCoroutine = null;
    }


    // ============================================================
    // FINISH TYPING
    // ============================================================

    void FinishTyping()
    {
        if (!isTyping)
            return;


        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);

            typingCoroutine = null;
        }


        CutsceneData.CutsceneLine currentLine =
            cutscene.dialogueLines[currentLineIndex];


        if (currentLine.speaker == 0)
        {
            character1Text.text =
                currentFullLine;
        }

        else if (currentLine.speaker == 1)
        {
            character2Text.text =
                currentFullLine;
        }


        isTyping = false;
    }


    // ============================================================
    // END CUTSCENE
    // ============================================================

    void EndCutscene()
    {
        character1Bubble.SetActive(false);
        character2Bubble.SetActive(false);

        Debug.Log("Cutscene finished.");


        // Mark intro/tutorial as completed
        PlayerDataManager.Instance.data.tutorialCompleted = true;
        PlayerDataManager.Instance.Save();


        // Find the transition controller in this scene
        SceneTransitionController transition =
            FindObjectOfType<SceneTransitionController>();


        if (transition != null)
        {
            transition.TriggerTransition(cutscene.nextScene);
        }
        else
        {
            Debug.LogError(
                "CutsceneManager: No SceneTransitionController found!"
            );
        }
    }
}