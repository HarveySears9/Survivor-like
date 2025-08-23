using UnityEngine;
using System.Collections;

public class AnimateWidthToParent : MonoBehaviour
{
    public RectTransform childRectTransform; // Reference to the child's RectTransform
    public RectTransform parentRectTransform; // Reference to the parent's RectTransform

    public float animationDuration = 0.5f; // Time it takes to adjust the width
    private Coroutine widthAdjustmentCoroutine;
    private Vector2 originalSize;

    public GameObject Upgrades;
    public AnimateImage animator;

    public GameObject[] activateAfterOpen;

    void Start()
    {
        // Get and store the original size of the child
        if (childRectTransform != null)
        {
            originalSize = childRectTransform.sizeDelta;
        }

        if (childRectTransform == null || parentRectTransform == null)
        {
            Debug.LogError("AnimateWidthToParent: Missing RectTransform components!");
        }
    }

    void OnEnable()
    {
        // Start animating the width of the child to match the parent's width
        StartWidthAnimation();

        // Restart sprite animation
        if (animator != null)
        {
            animator.animating = true;
            animator.StartAnimation(); // Ensure animation is restarted
        }
    }

    void OnDisable()
    {
        // Stop the width adjustment coroutine
        if (widthAdjustmentCoroutine != null)
        {
            StopCoroutine(widthAdjustmentCoroutine);
            widthAdjustmentCoroutine = null;
        }

        // Reset the child RectTransform's size to its original size
        if (childRectTransform != null)
        {
            childRectTransform.sizeDelta = originalSize;
        }

        // Stop sprite animation
        if (animator != null)
        {
            animator.StopAnimation();
        }

        Upgrades.SetActive(false);
    }

    void StartWidthAnimation()
    {
        if (childRectTransform != null && parentRectTransform != null)
        {
            if (widthAdjustmentCoroutine != null)
                StopCoroutine(widthAdjustmentCoroutine);

            widthAdjustmentCoroutine = StartCoroutine(AnimateWidth());
        }
        else
        {
            Debug.LogError("AnimateWidthToParent: RectTransform references are missing!");
        }
    }

    IEnumerator AnimateWidth()
    {
        float elapsedTime = 0f;

        // Get the initial and target widths
        float initialWidth = childRectTransform.sizeDelta.x;
        float targetWidth = parentRectTransform.rect.width;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime for time scale independence
            float newWidth = Mathf.Lerp(initialWidth, targetWidth, elapsedTime / animationDuration);

            // Update the child RectTransform's width
            childRectTransform.sizeDelta = new Vector2(newWidth, childRectTransform.sizeDelta.y);

            yield return null;
        }

        // Ensure the final width matches exactly
        childRectTransform.sizeDelta = new Vector2(targetWidth, childRectTransform.sizeDelta.y);

        animator.animating = false;
        if(Upgrades != null)
        {
            Upgrades.SetActive(true);
        }

        if (activateAfterOpen != null)
        {
            foreach (GameObject go in activateAfterOpen)
            {
                if (go != null) // safety check
                {
                    go.SetActive(true);
                }
            }
        }

    }
}