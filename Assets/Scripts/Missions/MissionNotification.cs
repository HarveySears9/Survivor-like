using UnityEngine;

public class MissionNotification : MonoBehaviour
{
    public GameObject exclamationPoint;

    void Start()
    {
        UpdateNotification();
    }

    public void UpdateNotification()
    {
        bool hasUnclaimedMission = false;

        if (PlayerDataManager.Instance != null &&
            PlayerDataManager.Instance.data.activeMissions != null)
        {
            foreach (Mission mission in PlayerDataManager.Instance.data.activeMissions)
            {
                if (mission.completed && !mission.claimed)
                {
                    hasUnclaimedMission = true;
                    break;
                }
            }
        }

        exclamationPoint.SetActive(hasUnclaimedMission);
    }
}