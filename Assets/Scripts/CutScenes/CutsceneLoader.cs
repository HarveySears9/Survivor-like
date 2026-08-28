using UnityEngine;

public class CutsceneLoader : MonoBehaviour
{
    public static CutsceneLoader Instance;

    private CutsceneData cutsceneToPlay;

    [Header("Scene Transition")]
    public SceneTransitionController sceneTransition;


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


    public void PlayCutscene(CutsceneData cutscene)
    {
        if (cutscene == null)
        {
            Debug.LogError(
                "CutsceneLoader: CutsceneData is null!"
            );

            return;
        }


        cutsceneToPlay = cutscene;


        // Use your existing transition system
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


    public CutsceneData GetCutscene()
    {
        return cutsceneToPlay;
    }
}