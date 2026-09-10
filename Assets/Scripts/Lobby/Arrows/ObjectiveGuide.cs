using UnityEngine;

public class ObjectiveGuide : MonoBehaviour
{
    [Header("Arrow")]
    public ObjectiveArrow arrow;

    [Header("Target")]
    public Transform target;

    [Header("Location Cutscene")]
    public string cutsceneID;

    [Header("Settings")]
    public bool alwaysShow = false;

    void Start()
    {
        UpdateArrow();
    }

    public void UpdateArrow()
    {
        // Some objectives should always be visible
        if (alwaysShow)
        {
            arrow.SetTarget(target);
            return;
        }

        // Show arrow until the location's cutscene has been seen
        if (!HasSeenCutscene())
        {
            arrow.SetTarget(target);
        }
        else
        {
            arrow.HideArrow();
        }
    }

    bool HasSeenCutscene()
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
}