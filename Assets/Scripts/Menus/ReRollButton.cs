using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReRollButton : MonoBehaviour
{
    public GameObject Scroll;

    private Button reRollButton;
    public TextMeshProUGUI buttonText;

    private bool hasRerolled = false;

    void Start()
    {
        reRollButton = GetComponent<Button>();
    }

    void OnEnable()
    {
        hasRerolled = false;

        StartCoroutine(EnableAfterScroll());
        buttonText.text = "Reroll?";
    }

    void OnDisable()
    {
        reRollButton.interactable = false;
    }

    IEnumerator EnableAfterScroll()
    {
        yield return new WaitForSecondsRealtime(
            Scroll.GetComponent<AnimateWidthToParent>().animationDuration
        );

        // Don't enable it if the player has already rerolled
        if (!hasRerolled)
        {
            reRollButton.interactable = true;
        }
    }

    public void OnClick()
    {
        if (AdsManager.Instance == null)
            return;

        if (hasRerolled)
            return;

        reRollButton.interactable = false;

        bool adStarted = AdsManager.Instance.ShowRewardedAd(
            PerformReroll
        );

        if (!adStarted)
        {
            reRollButton.interactable = true;
        }
    }

    private void PerformReroll()
    {
        hasRerolled = true;

        Scroll.SetActive(false);
        Scroll.SetActive(true);

        // Keep the button disabled permanently for this level-up
        reRollButton.interactable = false;
        buttonText.text = "Reroll Used";
    }
}