using UnityEngine;
using TMPro;

public class CutsceneUnlockPopup : MonoBehaviour
{
    [Header("UI")]
    public GameObject popup;
    public TMP_Text titleText;
    public TMP_Text equipmentNameText;

    private bool waitingForContinue = false;

    void Start()
    {
        Hide();
    }

    public void ShowEquipmentUnlock(string equipmentName)
    {
        popup.SetActive(true);

        titleText.text = "EQUIPMENT UNLOCKED!";

        equipmentNameText.text =
            equipmentName;

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