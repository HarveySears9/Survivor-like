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

    // Start is called before the first frame update
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
            if(!isMoving)
            {
                // Use modulo to cycle through the sprite array
                currentIndex = (currentIndex + 1) % spriteArray.Length;
                spriteRenderer.sprite = spriteArray[currentIndex];  // Update the sprite of the SpriteRenderer
            }
            else
            {
                // Use modulo to cycle through the sprite array
                currentIndex = (currentIndex + 1) % moveArray.Length;
                spriteRenderer.sprite = moveArray[currentIndex];  // Update the sprite of the SpriteRenderer
            }
        }
    }
}
