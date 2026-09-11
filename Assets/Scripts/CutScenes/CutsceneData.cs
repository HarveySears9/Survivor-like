using UnityEngine;

[CreateAssetMenu(
    fileName = "New Cutscene",
    menuName = "Game/Cutscene"
)]
public class CutsceneData : ScriptableObject
{
    public enum PlaybackType
    {
        EveryTime,
        Once
    }

    public enum UnlockType
    {
        Equipment,
        Weapon
    }

    [System.Serializable]
    public class CutsceneLine
    {
        // 0 = Character 1
        // 1 = Character 2
        // 2 = Character 3
        public int speaker;

        [TextArea(2, 5)]
        public string line;
    }

    [Header("Cutscene Settings")]
    public string cutsceneID;
    public PlaybackType playbackType = PlaybackType.Once;
    public string nextScene;

    [Header("Background")]
    public GameObject background;

    [Header("Characters")]
    public GameObject character1;
    public GameObject character2;
    public GameObject character3;

    [Header("Dialogue")]
    public CutsceneLine[] dialogueLines;

    [Header("Unlock")]
    public bool unlockItem;

    public UnlockType unlockType;

    public int itemIndex;

    public string itemName;

    [TextArea(2, 5)]
    public string itemDescription;

    public Sprite itemIcon;
}