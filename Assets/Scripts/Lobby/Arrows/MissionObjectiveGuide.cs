using UnityEngine;

public class MissionObjectiveGuide : MonoBehaviour
{
    [Header("Arrow")]
    public ObjectiveArrow arrow;

    [Header("Target")]
    public Transform target;

    [Header("Mission Board Cutscene")]
    public string cutsceneID = "MissionBoardIntro";

    void Start()
    {
        UpdateArrow();
    }

    public void UpdateArrow()
    {
        // First priority:
        // Guide the player to the Mission Board
        // if they haven't seen its introduction.
        if (!HasSeenCutscene())
        {
            arrow.SetTarget(target);
            return;
        }

        // After the introduction, only show the arrow
        // when there is a mission ready to claim.
        if (HasUnclaimedMission())
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

    bool HasUnclaimedMission()
    {
        if (PlayerDataManager.Instance == null)
            return false;

        if (
            PlayerDataManager.Instance.data.activeMissions
            == null
        )
        {
            return false;
        }

        foreach (
            Mission mission
            in PlayerDataManager.Instance.data.activeMissions
        )
        {
            if (mission.completed && !mission.claimed)
            {
                return true;
            }
        }

        return false;
    }
}