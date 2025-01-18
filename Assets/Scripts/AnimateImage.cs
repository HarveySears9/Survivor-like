using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimateImage : MonoBehaviour
{
    public Sprite[] spriteArray;        // Array of sprites to animate
    private Image imageComponent;       // Reference to the Image component
    public bool animating = true;       // Whether the animation is active
    private int currentIndex = 0;       // Current sprite index
    public float animationSpeed = 0.25f; // Time between sprite updates in seconds

    private Coroutine animationCoroutine; // Reference to the running animation coroutine

    public AnimateImage[] syncWith;

    void OnEnable()
    {
        imageComponent = GetComponent<Image>();

        if (animating)
        {
            StartAnimation();
        }
    }

    void OnDisable()
    {
        //StopAnimation(); // Stop animation when disabled
    }

    public void StartAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimateSprites());
    }

    public void StopAnimation()
    {
        animating = false;

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    IEnumerator AnimateSprites()
    {
        while (animating)
        {
            if (imageComponent != null)
            {
                if (syncWith != null && syncWith.Length > 0)
                {
                    // Loop through the sync array and match the currentIndex with the first valid one
                    foreach (AnimateImage sync in syncWith)
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
                    // cycle through the appropriate sprite array
                    currentIndex = (currentIndex + 1) % spriteArray.Length;
                }
                imageComponent.sprite = spriteArray[currentIndex];
            }

            // Wait for the specified time without being affected by Time.timeScale
            yield return new WaitForSecondsRealtime(animationSpeed);
        }
    }
}
