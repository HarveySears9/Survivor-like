using UnityEngine;
using TMPro;
using System.Collections;

public class NPCs : MonoBehaviour
{
    private SpriteRenderer sr;
    private Transform player;

    public float flipRange = 5f;
    public bool idle = true;

    [Header("Dialogue Settings")]
    public string[] dialogueLines;           // Lines this NPC can say
    public GameObject dialogueBubblePrefab;  // Assign in Inspector
    public float typeSpeed = 0.05f;          // Speed of typing
    public float dialogBoxHieght = 1f;

    private GameObject currentBubble;
    private TextMeshProUGUI dialogueText;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(FlipSprite());
    }

    IEnumerator FlipSprite()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, flipRange));
            if (idle)
            {
                sr.flipX = !sr.flipX;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.transform;
        FacePlayer();

        // Show dialogue bubble
        if (dialogueBubblePrefab != null && currentBubble == null)
        {
            currentBubble = Instantiate(dialogueBubblePrefab, transform);
            currentBubble.transform.localPosition = new Vector3(0, dialogBoxHieght, 0);

            dialogueText = currentBubble.GetComponentInChildren<TextMeshProUGUI>();
            dialogueText.text = "";
            StartCoroutine(TypeLine(dialogueLines[Random.Range(0, dialogueLines.Length)]));

        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        FacePlayer();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        idle = true;
        player = null;
        dialogueText.text = "";

        // Destroy bubble on exit
        if (currentBubble != null)
        {
            Destroy(currentBubble);
        }
    }

    private void FacePlayer()
    {
        if (player != null)
        {
            idle = false;
            float direction = player.position.x - transform.position.x;
            sr.flipX = direction < 0;
        }
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char c in line.ToCharArray())
        {
            if (currentBubble != null)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typeSpeed);
            }
            else
            {
                yield break;
            }
        }
    }
}
