using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for Button
using TMPro;          // Required for TextMeshProUGUI
using UnityEngine.SceneManagement;

public class Tavern : MonoBehaviour
{
    public GameObject[] skinMenus;      // Array of skin menus for each character
    public Button[] selectButtons;     // Array of "Select" buttons for each character

    // Start is called before the first frame update
    void Start()
    {
        // Load the current character index from the save file
        int characterIndex = SaveFile.LoadData<SaveFile.Data>().currentCharacter;

        // Activate the skin menu for the current character
        if (skinMenus != null && characterIndex < skinMenus.Length)
        {
            skinMenus[characterIndex].SetActive(true);
        }

        // Disable the "Select" button for the current character
        if (selectButtons != null && characterIndex < selectButtons.Length)
        {
            selectButtons[characterIndex].interactable = false;
        }

        // Update button text for all buttons
        foreach (Button button in selectButtons)
        {
            if (button != null)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Select";
            }
        }

        // Set the current character's button to "Selected"
        if (selectButtons != null && characterIndex < selectButtons.Length)
        {
            selectButtons[characterIndex].GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
        }
    }

    public void SetSelectionButtons(int characterIndex)
    {
        // Update button text for all buttons
        foreach (Button button in selectButtons)
        {
            if (button != null)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Select";
                button.interactable = true;
            }
        }

        selectButtons[characterIndex].interactable = false;
        selectButtons[characterIndex].GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
    }

    public void Home()
    {
        if (Application.CanStreamedLevelBeLoaded("PuddleBrook"))
        {
            SceneTracker.UpdateLastSceneName();
            SceneManager.LoadScene("PuddleBrook");
        }
        else
        {
            Debug.LogError("Scene 'MainMenu' not found. Please check Build Settings.");
        }
    }
}
