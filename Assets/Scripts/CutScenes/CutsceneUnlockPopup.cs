using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CutsceneUnlockPopup : MonoBehaviour
{
    [Header("UI")]
    public GameObject popup;

    public TMP_Text titleText;
    public TMP_Text equipmentNameText;
    public TMP_Text descriptionText;

    public Image itemIconImage;

    private bool waitingForContinue = false;

    void Start()
    {
        Hide();
    }

    public void ShowUnlock(
        string itemName,
        string itemDescription,
        Sprite itemIcon,
        bool isWeapon
    )
    {
        popup.SetActive(true);

        if (isWeapon)
        {
            titleText.text = "NEW WEAPON UNLOCKED!";
        }
        else
        {
            titleText.text = "NEW EQUIPMENT UNLOCKED!";
        }

        equipmentNameText.text = itemName;

        if (descriptionText != null)
        {
            descriptionText.text = itemDescription;
        }

        if (itemIconImage != null)
        {
            itemIconImage.sprite = itemIcon;
            itemIconImage.enabled = itemIcon != null;
        }

        waitingForContinue = true;
    }

    public void Hide()
    {
        popup.SetActive(false);
        waitingForContinue = false;
    }

    public bool IsWaitingForContinue()
    {
        return waitingForContinue;
    }
}