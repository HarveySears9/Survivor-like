using UnityEngine;

public class CutsceneLoader : MonoBehaviour
{
    [Header("Scene Transition")]
    public SceneTransitionController sceneTransition;

    public void PlayCutscene(CutsceneData cutscene)
    {
        if (cutscene == null)
        {
            Debug.LogError(
                "CutsceneLoader: CutsceneData is null!"
            );
            return;
        }

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

                return;
            }
        }

        CutsceneDataHolder.cutsceneToPlay = cutscene;

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

    public bool HasCompletedCutscene(string cutsceneID)
    {
        if (PlayerDataManager.Instance == null)
            return false;

        if (PlayerDataManager.Instance.data.completedCutscenes == null)
            return false;

        return PlayerDataManager.Instance.data.completedCutscenes
            .Contains(cutsceneID);
    }

    public void MarkCutsceneCompleted(string cutsceneID)
    {
        if (PlayerDataManager.Instance == null)
            return;

        if (PlayerDataManager.Instance.data.completedCutscenes == null)
        {
            PlayerDataManager.Instance.data.completedCutscenes =
                new System.Collections.Generic.List<string>();
        }

        if (!PlayerDataManager.Instance.data.completedCutscenes
            .Contains(cutsceneID))
        {
            PlayerDataManager.Instance.data.completedCutscenes
                .Add(cutsceneID);

            PlayerDataManager.Instance.Save();
        }
    }

    public void ApplyCutsceneUnlocks(CutsceneData cutscene)
    {
        if (cutscene == null)
            return;

        if (PlayerDataManager.Instance == null)
            return;

        if (!cutscene.unlockItem)
            return;

        SaveFile.Data data =
            PlayerDataManager.Instance.data;

        int index = cutscene.itemIndex;

        if (cutscene.unlockType == CutsceneData.UnlockType.Equipment)
        {
            if (data.equipmentUnlocks == null)
            {
                Debug.LogError(
                    "Equipment unlock array is null!"
                );
                return;
            }

            if (
                index < 0 ||
                index >= data.equipmentUnlocks.Length
            )
            {
                Debug.LogError(
                    "Invalid equipment unlock index: " +
                    index
                );
                return;
            }

            if (data.equipmentUnlocks[index])
            {
                Debug.Log(
                    "Equipment already unlocked: " +
                    cutscene.itemName
                );
                return;
            }

            data.equipmentUnlocks[index] = true;

            PlayerDataManager.Instance.Save();

            Debug.Log(
                "Equipment unlocked: " +
                cutscene.itemName
            );
        }
        else if (cutscene.unlockType == CutsceneData.UnlockType.Weapon)
        {
            if (data.weaponUnlocks == null)
            {
                Debug.LogError(
                    "Weapon unlock array is null!"
                );
                return;
            }

            if (
                index < 0 ||
                index >= data.weaponUnlocks.Length
            )
            {
                Debug.LogError(
                    "Invalid weapon unlock index: " +
                    index
                );
                return;
            }

            if (data.weaponUnlocks[index])
            {
                Debug.Log(
                    "Weapon already unlocked: " +
                    cutscene.itemName
                );
                return;
            }

            data.weaponUnlocks[index] = true;

            PlayerDataManager.Instance.Save();

            Debug.Log(
                "Weapon unlocked: " +
                cutscene.itemName
            );
        }
    }
}