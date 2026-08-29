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
    public CutsceneLoader cutsceneLoader;


    // ============================================================
    // SCENE REFERENCES
    // ============================================================

    [Header("Background")]

    public GameObject backgroundHolder;


    [Header("Characters")]

    public Transform character1Position;
    public Transform character2Position;
    public Transform character3Position;


    // ============================================================
    // SPEECH BUBBLES
    // ============================================================

    [Header("Speech Bubbles")]

    public GameObject character1Bubble;
    public GameObject character2Bubble;
    public GameObject character3Bubble;

    public TextMeshProUGUI character1Text;
    public TextMeshProUGUI character2Text;
    public TextMeshProUGUI character3Text;


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
    private GameObject currentCharacter3;
    private GameObject currentBackground;


    // ============================================================
    // START
    // ============================================================

    void Start()
    {
        cutscene = CutsceneDataHolder.cutsceneToPlay;

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

        // Create Character 3
        if (cutscene.character3 != null)
        {
            currentCharacter3 =
                Instantiate(
                    cutscene.character3,
                    character3Position
                );

            currentCharacter3.transform.localPosition =
                Vector3.zero;

            currentCharacter3.transform.localRotation =
                Quaternion.identity;

            // Flip Character 3 horizontally
            Vector3 scale = currentCharacter3.transform.localScale;
            scale.x *= -1f;
            currentCharacter3.transform.localScale = scale;
        }


        // Hide speech bubbles
        character1Bubble.SetActive(false);
        character2Bubble.SetActive(false);
        character3Bubble.SetActive(false);


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

        if (currentCharacter3 != null)
        {
            Destroy(currentCharacter3);
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

            CutsceneCharacter character =
            currentCharacter1.GetComponent<CutsceneCharacter>();

            if (character != null)
            {
                Vector3 bubblePosition =
                    currentCharacter1.transform.position;

                bubblePosition.y += character.speechBubbleHeight;

                character1Bubble.transform.position =
                    bubblePosition;
            }

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

            CutsceneCharacter character =
            currentCharacter2.GetComponent<CutsceneCharacter>();

            if (character != null)
            {
                Vector3 bubblePosition =
                    currentCharacter2.transform.position;

                bubblePosition.y += character.speechBubbleHeight;

                character2Bubble.transform.position =
                    bubblePosition;
            }

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

        // Character 3 speaking
        else if (currentLine.speaker == 2)
        {
            if (currentCharacter3 == null)
            {
                Debug.LogError(
                    "Character 3 is trying to speak, but no Character 3 exists!"
                );

                return;
            }

            character3Bubble.SetActive(true);


            // Get character bubble height
            CutsceneCharacter character =
                currentCharacter3.GetComponent<CutsceneCharacter>();


            if (character != null)
            {
                Vector3 bubblePosition =
                    currentCharacter3.transform.position;

                bubblePosition.y +=
                    character.speechBubbleHeight;

                character3Bubble.transform.position =
                    bubblePosition;
            }


            currentFullLine =
                currentLine.line;


            typingCoroutine =
                StartCoroutine(
                    TypeLine(
                        currentFullLine,
                        character3Text
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

        else if (currentLine.speaker == 2)
        {
            character3Text.text =
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
        character3Bubble.SetActive(false);

        Debug.Log("Cutscene finished.");


        // Mark the cutscene as completed
        if (
            cutscene.playbackType ==
            CutsceneData.PlaybackType.Once
        )
        {
            cutsceneLoader.MarkCutsceneCompleted(
                cutscene.cutsceneID
            );
        }


        // Load the next scene
        SceneTransitionController transition =
            FindObjectOfType<SceneTransitionController>();


        if (transition != null)
        {
            transition.TriggerTransition(
                cutscene.nextScene
            );
        }
        else
        {
            Debug.LogError(
                "CutsceneManager: No SceneTransitionController found!"
            );
        }
    }
}