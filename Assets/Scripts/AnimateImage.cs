using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimateImage : MonoBehaviour
{
    public Sprite[] spriteArray;        // Array of sprites to animate
    private Image imageComponent;       // Reference to the Image component
    public bool animating = true;       // Whether the animation is active
    private int currentIndex = 0;       // Current sprite index
    public float animationSpeed = 0.25f; // Time between sprite updates

    private Coroutine animationCoroutine;

    public AnimateImage masterSprite;  // Reference to the master AnimateSprite for syncing

    public int CurrentIndex => currentIndex;

    void OnEnable()
    {
        imageComponent = GetComponent<Image>();
        if (animating)
        {
            StartAnimation();
        }
        currentIndex = 0;
    }

    public void StartAnimation()
    {
        if(!animating)
        {
            animating = true;
        }
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimateSprites());
    }

    public void ContinueAnimation()
    {
        animating = true;
    }
    public void PauseAnimation()
    {
        animating = false;
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
                // Sync with master sprite if one exists
                if (masterSprite != null && masterSprite.animating)
                {
                    currentIndex = masterSprite.CurrentIndex;
                }
                else
                {
                    currentIndex = (currentIndex + 1) % spriteArray.Length;
                }

                imageComponent.sprite = spriteArray[currentIndex];
            }

            // Wait for the specified time without being affected by Time.timeScale
            yield return new WaitForSecondsRealtime(animationSpeed);
        }
    }
}
