using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MissionUI : MonoBehaviour
{
    public SceneTransitionController stc;

    public Sprite coinGFX;

    [Header("Scroll Contents")]
    public Transform dailyScrollContent;
    public Transform weeklyScrollContent;

    public GameObject missionUIPrefab;

    private List<GameObject> spawnedMissions = new List<GameObject>();

    void Start()
    {
        RefreshMissionUI();
        ResizeContent(dailyScrollContent);
        ResizeContent(weeklyScrollContent);
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

                ui.progressSlider.value = mission.Progress01;   // value between 0 and 1
                ui.progressText.text = "Progress: " + Mathf.RoundToInt(mission.Progress01 * 100) + "%";


                ui.claimButton.interactable = !mission.claimed && mission.completed;
                ui.claimButton.onClick.RemoveAllListeners();
                ui.claimButton.onClick.AddListener(() =>
                {
                    MissionManager.Instance.ClaimMission(mission);
                });
                
                ui.claimButton.interactable = !mission.claimed && mission.completed;

                var buttonText = ui.claimButton.GetComponentInChildren<TextMeshProUGUI>();
                if (mission.claimed)
                    buttonText.text = "Claimed";
                else if (mission.completed)
                    buttonText.text = "Claim";
                else
                    buttonText.text = "Incomplete";

            }
        }
    }

    private void ResizeContent(Transform scrollContent)
    {
        int childCount = scrollContent.transform.childCount;
        float elementHeight = 500f; // your prefab’s height
        float spacing = 50f;             // optional, if using a VerticalLayoutGroup

        float newHeight = (childCount * elementHeight) + (childCount - 1) * spacing + 350f;

        RectTransform rt = scrollContent.GetComponent<RectTransform>();
        if (rt != null)
        {
            Vector2 size = rt.sizeDelta;
            size.y = newHeight;
            rt.sizeDelta = size;
        }
    }

    public void Home()
    {
        if (Application.CanStreamedLevelBeLoaded("PuddleBrook"))
        {
            SceneTracker.UpdateLastSceneName();
            stc.TriggerTransition("PuddleBrook");
        }
        else
        {
            Debug.LogError("Scene 'Puddlebrook' not found. Please check Build Settings.");
        }
    }
}
