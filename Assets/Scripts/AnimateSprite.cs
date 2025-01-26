using System.Collections;
using UnityEngine;

public class AnimateSprite : MonoBehaviour
{
    public Sprite[] spriteArray;        // Array of sprites for animation
    public Sprite[] moveArray;          // Array of sprites for animation
    private SpriteRenderer spriteRenderer; // SpriteRenderer component for displaying sprites
    public bool animating;              // Whether the animation is active
    private int currentIndex = 0;       // Current index of the sprite
    public float animationSpeed = 0.25f; // Time between sprite updates
    public bool isMoving = false;

    // Reference to another AnimateSprite instance to sync with
    public AnimateSprite masterSprite;

    public int CurrentIndex => currentIndex;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (animating)
        {
            StartAnimation();
        }
    }

    public void StartAnimation()
    {
        animating = true;
        InvokeRepeating(nameof(UpdateSprite), 0f, animationSpeed);
    }

    public void StopAnimation()
    {
        animating = false;
        CancelInvoke(nameof(UpdateSprite));
    }

    void UpdateSprite()
    {
        if (!animating || spriteRenderer == null) return;

        // Sync with master sprite if one exists
        if (masterSprite != null && masterSprite.animating)
        {
            currentIndex = masterSprite.currentIndex;
        }
        else
        {
            currentIndex = (currentIndex + 1) % (isMoving ? moveArray.Length : spriteArray.Length);
        }

        spriteRenderer.sprite = isMoving ? moveArray[currentIndex] : spriteArray[currentIndex];
    }
}
