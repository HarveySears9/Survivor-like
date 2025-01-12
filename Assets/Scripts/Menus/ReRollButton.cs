using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReRollButton : MonoBehaviour
{
    public GameObject Scroll;        // Reference to the scroll object
    private Button reRollButton;     // Reference to the reroll button

    void Start()
    {
        reRollButton = GetComponent<Button>();  // Get the Button component attached to this GameObject
    }

    void OnEnable()
    {
        StartCoroutine(EnableAfterScroll());
    }

    void OnDisable()
    {
        reRollButton.interactable = false;
    }

    IEnumerator EnableAfterScroll()
    {
        // Wait for the animation to complete (based on the duration of the animation)
        yield return new WaitForSecondsRealtime(Scroll.GetComponent<AnimateWidthToParent>().animationDuration);

        // Re-enable the button after the animation is done
        reRollButton.interactable = true;
    }

    public void OnClick()
    {
        // Disable the button temporarily while the animation is happening
        reRollButton.interactable = false;

        // Deactivate and reactivate the Scroll (or whatever UI element you're working with)
        Scroll.SetActive(false);  // Deactivate the scroll
        Scroll.SetActive(true);   // Reactivate the scroll
        StartCoroutine(EnableAfterScroll());
    }
}
