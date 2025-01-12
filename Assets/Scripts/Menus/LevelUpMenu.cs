using UnityEngine;

public class LevelUpMenu : MonoBehaviour
{
    void OnEnable()
    {
        // Pause the game when the menu is displayed
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        // Resume the game when the menu is closed
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
