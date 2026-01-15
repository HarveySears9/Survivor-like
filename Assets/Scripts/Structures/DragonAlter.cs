using UnityEngine;

public class DragonAlter : MonoBehaviour
{
    private AnimateSprite animator;
    public bool isActive = true;

    private InteractButton button;
    private DragonAlterMenu menu;

    void Awake()
    {
        animator = GetComponent<AnimateSprite>();

        // Find the menu at runtime
        menu = FindObjectOfType<DragonAlterMenu>(true);
        if (menu == null)
        {
            Debug.LogError("DragonAlterMenu not found in scene!");
        }
        button = FindObjectOfType<InteractButton>(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (animator != null)
        {
            if (isActive)
            {
                animator.isMoving = true;
                button.gameObject.SetActive(true);
                button.SetIndex(0);
                menu.SetAlter(this);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (animator != null)
        {
            animator.isMoving = false;
            button.gameObject.SetActive(false);
            button.SetIndex(-1);
            menu.SetAlter(null);
        }
    }

    public void TakeDeal()
    {
        isActive = false;
        animator.StopAnimation();
        animator.spriteRenderer.sprite = animator.spriteArray[0];
        button.SetIndex(-1);
    }
}
