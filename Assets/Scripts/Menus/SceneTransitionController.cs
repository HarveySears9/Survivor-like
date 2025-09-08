using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionController : MonoBehaviour
{
    public CanvasGroup transitionScreenCanvasGroup; // Reference to the CanvasGroup of the transition screen
    public float transitionDuration = 2.0f; // Duration of the transition effect

    void Start()
    {
        // Ensure the transition screen is initially hidden
        transitionScreenCanvasGroup.alpha = 1f;
        transitionScreenCanvasGroup.interactable = false;
        transitionScreenCanvasGroup.blocksRaycasts = false;

        StartCoroutine(TransitionInToScene());
    }

    public void TriggerTransition(string scene)
    {
        StartCoroutine(TransitionToNextScene(scene));
    }

    IEnumerator TransitionInToScene()
    {
        // Fade out the transition screen
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            transitionScreenCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / transitionDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the transition screen is fully hidden
        transitionScreenCanvasGroup.alpha = 0f;
        transitionScreenCanvasGroup.interactable = false;
        transitionScreenCanvasGroup.blocksRaycasts = false;
    }

    IEnumerator TransitionToNextScene(string scene)
    {
        float elapsedTime = 0f;

        // Fade in the transition screen
        while (elapsedTime < transitionDuration)
        {
            transitionScreenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / transitionDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the transition screen is fully visible
        transitionScreenCanvasGroup.alpha = 1f;

        // Load the next scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = true;

        // Wait until the next scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}