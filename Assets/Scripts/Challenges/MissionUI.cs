using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MissionUI : MonoBehaviour
{
    public Sprite coinGFX;

    [Header("Scroll Contents")]
    public Transform dailyScrollContent;
    public Transform weeklyScrollContent;

    public GameObject missionUIPrefab;

    private List<GameObject> spawnedMissions = new List<GameObject>();

    void Start()
    {
        RefreshMissionUI();
    }

    public void RefreshMissionUI()
    {
        // Clear old UI
        foreach (var go in spawnedMissions)
            Destroy(go);
        spawnedMissions.Clear();

        var missions = PlayerDataManager.Instance.data.activeMissions;

        foreach (var mission in missions)
        {
            // Choose parent based on type
            Transform parent = mission.type == ChallengeType.Daily ? dailyScrollContent : weeklyScrollContent;

            GameObject instance = Instantiate(missionUIPrefab, parent);
            spawnedMissions.Add(instance);

            MissionUIPrefab ui = instance.GetComponent<MissionUIPrefab>();
            if (ui != null)
            {
                ui.MissionText.text = mission.missionText;
                if (mission.rewardType == RewardType.Coins)
                {
                    ui.missionGFX.sprite = coinGFX;
                    ui.rewardAmount.text = "x" + mission.rewardAmount;
                }

                /*ui.claimButton.interactable = !mission.claimed;
                ui.claimButton.onClick.RemoveAllListeners();
                ui.claimButton.onClick.AddListener(() =>
                {
                    ClaimMission(mission, ui);
                });*/
            }
        }
    }
}
