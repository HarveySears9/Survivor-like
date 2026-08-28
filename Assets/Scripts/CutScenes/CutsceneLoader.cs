using UnityEngine;

public class CutsceneLoader : MonoBehaviour
{
    public static CutsceneLoader Instance;

    private CutsceneData cutsceneToPlay;

    [Header("Scene Transition")]
    public SceneTransitionController sceneTransition;


    // ============================================================
    // AWAKE
    // ============================================================

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // ============================================================
    // PLAY CUTSCENE
    // ============================================================

    public void PlayCutscene(CutsceneData cutscene)
    {
        if (cutscene == null)
        {
            Debug.LogError(
                "CutsceneLoader: CutsceneData is null!"
            );

            return;
        }


        // Make sure the cutscene has an ID
        if (string.IsNullOrEmpty(cutscene.cutsceneID))
        {
            Debug.LogError(
                "CutsceneLoader: Cutscene has no ID!"
            );

            return;
        }


        if (cutscene.playbackType == CutsceneData.PlaybackType.Once)
        {
            if (HasCompletedCutscene(cutscene.cutsceneID))
            {
                Debug.Log(
                    "Cutscene already completed: " +
                    cutscene.cutsceneID
                );

                // Skip the cutscene and go straight
                // to the scene it would have loaded.
                if (!string.IsNullOrEmpty(cutscene.nextScene))
                {
                    if (sceneTransition != null)
                    {
                        sceneTransition.TriggerTransition(
                            cutscene.nextScene
                        );
                    }
                    else
                    {
                        Debug.LogError(
                            "CutsceneLoader: SceneTransitionController is not assigned!"
                        );
                    }
                }
                else
                {
                    Debug.LogError(
                        "CutsceneLoader: Cutscene has no next scene!"
                    );
                }

                return;
            }
        }


        // Remember which cutscene we want to play
        cutsceneToPlay = cutscene;


        // Load the cutscene scene using your transition
        if (sceneTransition != null)
        {
            sceneTransition.TriggerTransition("Cutscene");
        }
        else
        {
            Debug.LogError(
                "CutsceneLoader: SceneTransitionController is not assigned!"
            );
        }
    }


    // ============================================================
    // GET CUTSCENE
    // ============================================================

    public CutsceneData GetCutscene()
    {
        return cutsceneToPlay;
    }


    // ============================================================
    // CHECK COMPLETED
    // ============================================================

    public bool HasCompletedCutscene(string cutsceneID)
    {
        if (PlayerDataManager.Instance == null)
            return false;


        if (
            PlayerDataManager.Instance.data.completedCutscenes
            == null
        )
        {
            return false;
        }


        return PlayerDataManager.Instance.data.completedCutscenes
            .Contains(cutsceneID);
    }


    // ============================================================
    // MARK COMPLETED
    // ============================================================

    public void MarkCutsceneCompleted(string cutsceneID)
    {
        if (PlayerDataManager.Instance == null)
            return;


        if (
            PlayerDataManager.Instance.data.completedCutscenes
            == null
        )
        {
            PlayerDataManager.Instance.data.completedCutscenes =
                new System.Collections.Generic.List<string>();
        }


        // Don't add duplicates
        if (
            !PlayerDataManager.Instance.data.completedCutscenes
                .Contains(cutsceneID)
        )
        {
            PlayerDataManager.Instance.data.completedCutscenes
                .Add(cutsceneID);

            PlayerDataManager.Instance.Save();
        }
    }
}