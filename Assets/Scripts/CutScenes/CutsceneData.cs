using UnityEngine;

[CreateAssetMenu(
    fileName = "New Cutscene",
    menuName = "Game/Cutscene"
)]
public class CutsceneData : ScriptableObject
{
    // ============================================================
    // PLAYBACK TYPE
    // ============================================================

    public enum PlaybackType
    {
        EveryTime,
        Once
    }


    // ============================================================
    // DIALOGUE LINE
    // ============================================================

    [System.Serializable]
    public class CutsceneLine
    {
        // 0 = Character 1
        // 1 = Character 2
        public int speaker;

        [TextArea(2, 5)]
        public string line;
    }


    // ============================================================
    // CUTSCENE INFORMATION
    // ============================================================

    [Header("Cutscene Settings")]

    public string cutsceneID;

    public PlaybackType playbackType = PlaybackType.Once;

    public string nextScene;


    // ============================================================
    // BACKGROUND
    // ============================================================

    [Header("Background")]

    public GameObject background;


    // ============================================================
    // CHARACTERS
    // ============================================================

    [Header("Characters")]

    public GameObject character1;
    public GameObject character2;
    public GameObject character3;


    // ============================================================
    // DIALOGUE
    // ============================================================

    [Header("Dialogue")]

    public CutsceneLine[] dialogueLines;
}