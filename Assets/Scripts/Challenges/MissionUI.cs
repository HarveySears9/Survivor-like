using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionUI : MonoBehaviour
{
    /*
    public TextMeshProUGUI missionText;
    public Slider progressBar;
    public TextMeshProUGUI progressPercent;
    public Button claimButton;
    public Image rewardImage;
    public TextMeshProUGUI rewardText;

    private Mission mission;

    public void SetMission(Mission m)
    {
        mission = m;
        Refresh();
    }

    public void Refresh()
    {
        if (mission == null) return;

        missionText.text = mission.missionText;
        progressBar.value = mission.Progress01;
        progressPercent.text = $"{mission.currentAmount}/{mission.targetAmount}";
        rewardImage.sprite = mission.rewardImage;
        rewardText.text = mission.rewardText;

        claimButton.interactable = mission.completed && !mission.claimed;
    }

    public void OnClaim()
    {
        if (mission != null && mission.completed && !mission.claimed)
        {
            mission.claimed = true;

            // Give reward here
            if (mission.rewardText.StartsWith("x")) // coins
            {
                int coins = int.Parse(mission.rewardText.Substring(1));
                PlayerDataManager.Instance.data.coins += coins;
            }
            else if (mission.rewardText.Contains("Skin"))
            {
                // Example: unlock gold skin
                PlayerDataManager.Instance.data.skins[3].owned = true;
            }

            PlayerDataManager.Instance.Save();

            Debug.Log($"Claimed reward: {mission.rewardText}");
            Refresh();
        }
    }
    */
}
