using UnityEngine;

[CreateAssetMenu(
    fileName = "New Cutscene",
    menuName = "Game/Cutscene"
)]
public class CutsceneData : ScriptableObject
{
    [System.Serializable]
    public class CutsceneLine
    {
        // 0 = Character 1
        // 1 = Character 2
        public int speaker;

        [TextArea(2, 5)]
        public string line;
    }


    [Header("Scene To Load After Cutscene")]

    public string nextScene;


    [Header("Background")]

    public GameObject background;


    [Header("Characters")]

    public GameObject character1;
    public GameObject character2;


    [Header("Dialogue")]

    public CutsceneLine[] dialogueLines;
}