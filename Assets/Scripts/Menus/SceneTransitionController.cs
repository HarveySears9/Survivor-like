using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionController : MonoBehaviour
{
    public CanvasGroup transitionScreenCanvasGroup; // Transition screen
    public float transitionDuration = 2.0f;         // Fade duration
    public Slider loadingSlider;                    // Loading bar slider
    public Image brick, shadow;

    void Start()
    {
        // Ensure transition screen is initially hidden
        transitionScreenCanvasGroup.alpha = 1f;
        transitionScreenCanvasGroup.interactable = false;
        transitionScreenCanvasGroup.blocksRaycasts = false;

        if (loadingSlider != null)
            loadingSlider.gameObject.SetActive(false); // Hide slider initially

        StartCoroutine(TransitionInToScene());
    }

    public void TriggerTransition(string scene)
    {
        StartCoroutine(TransitionToNextScene(scene));
    }

    IEnumerator TransitionInToScene()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            transitionScreenCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transitionScreenCanvasGroup.alpha = 0f;
        transitionScreenCanvasGroup.interactable = false;
        transitionScreenCanvasGroup.blocksRaycasts = false;
    }

    IEnumerator TransitionToNextScene(string scene)
    {
        if (loadingSlider != null)
        {
            loadingSlider.gameObject.SetActive(true);
            loadingSlider.value = 0f;
        }

        if(brick != null)
        {
            brick.enabled = true;
        }
        if (shadow != null)
        {
            shadow.enabled = true;
        }

        float elapsedTime = 0f;

        // Fade in transition screen
        while (elapsedTime < transitionDuration)
        {
            transitionScreenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transitionScreenCanvasGroup.alpha = 1f;

        // Load scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false; // We'll control activation

        while (!asyncLoad.isDone)
        {
            // Update slider (asyncLoad.progress goes 0–0.9)
            if (loadingSlider != null)
                loadingSlider.value = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // Activate scene when fully loaded
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
