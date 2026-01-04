using UnityEngine;

public class DragonAlter : MonoBehaviour
{
    private AnimateSprite animator;

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
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (animator != null)
        {
            animator.isMoving = false;
        }
    }
}
