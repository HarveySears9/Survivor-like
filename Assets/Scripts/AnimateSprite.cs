using System.Collections;
using UnityEngine;

public class AnimateSprite : MonoBehaviour
{
    public Sprite[] spriteArray;        // Array of sprites for animation
    public Sprite[] moveArray;        // Array of sprites for animation
    private SpriteRenderer spriteRenderer;  // SpriteRenderer component for displaying sprites
    public bool animating;              // Whether the animation is active
    private int currentIndex = 0;       // Current index of the sprite
    public float animationSpeed = 0.25f; // Time between sprite updates (can be adjusted in the Inspector)
    public bool isMoving = false;

    // Reference to another AnimateSprite instance to sync with
    public AnimateSprite syncWith;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        if (animating)
        {
            StartAnimation();
        }
    }

    // Start the animation
    public void StartAnimation()
    {
        animating = true;
        InvokeRepeating("UpdateSprite", 0f, animationSpeed);
    }

    // Stop the animation
    public void StopAnimation()
    {
        animating = false;
        CancelInvoke("UpdateSprite");
    }

    // Toggles between sprites to create an animation
    void UpdateSprite()
    {
        if (spriteRenderer != null && animating)
        {
            // If syncing with another AnimateSprite, match the index
            if (syncWith != null && syncWith.animating)
            {
                currentIndex = syncWith.currentIndex;
            }
            else
            {
                // Use modulo to cycle through the appropriate sprite array
                currentIndex = (currentIndex + 1) % (isMoving ? moveArray.Length : spriteArray.Length);
            }

            // Update the sprite of the SpriteRenderer
            spriteRenderer.sprite = isMoving ? moveArray[currentIndex] : spriteArray[currentIndex];
        }
    }
}
