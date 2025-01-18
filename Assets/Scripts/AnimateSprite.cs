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
    public AnimateSprite[] syncWith;

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
            // Check if there's an array of sync targets
            if (syncWith != null && syncWith.Length > 0)
            {
                // Loop through the sync array and match the currentIndex with the first valid one
                foreach (AnimateSprite sync in syncWith)
                {
                    if (sync != null && sync.animating)
                    {
                        currentIndex = sync.currentIndex;
                        break; // Sync with the first valid target and exit the loop
                    }
                }
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
