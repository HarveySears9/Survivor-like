using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTracker : MonoBehaviour
{
    public static string LastSceneName { get; private set; } = "";

    void Awake()
    {
        // Don't destroy the tracker when changing scenes
        DontDestroyOnLoad(gameObject);
    }

    // Call this method before loading a new scene
    public static void UpdateLastSceneName()
    {
        LastSceneName = SceneManager.GetActiveScene().name;
    }
}
