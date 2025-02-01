using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTracker : MonoBehaviour
{
    public static SceneTracker Instance { get; private set; }
    public static string LastSceneName { get; private set; } = "";

    void Awake()
    {
        if (Instance == null) // Check if an instance already exists
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Preserve this instance across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Call this method before loading a new scene
    public static void UpdateLastSceneName()
    {
        if (Instance != null) // Ensure Instance exists before updating
        {
            LastSceneName = SceneManager.GetActiveScene().name;
        }
    }
}
