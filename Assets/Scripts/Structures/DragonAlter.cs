using UnityEngine;

public class DragonAlter : MonoBehaviour
{
    private AnimateSprite animator;
    public GameObject button;

    void Start()
    {
        animator = GetComponent<AnimateSprite>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (animator != null)
        {
            animator.isMoving = true;
            button.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (animator != null)
        {
            animator.isMoving = false;
            button.SetActive(false);
        }
    }
}
