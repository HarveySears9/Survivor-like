using System;
using UnityEngine;

public class AuraObjectiveGuide : MonoBehaviour
{
    [Header("Arrow")]
    public ObjectiveArrow arrow;

    [Header("Target")]
    public Transform target;

    [Header("Aura Farm Cutscene")]
    public string cutsceneID = "AuraFarmIntro";

    [Header("Aura Settings")]
    public float fillTimeHours = 12f;

    private readonly float[] auraCapacityByLevel =
    {
        1000f,
        1500f,
        2250f
    };

    void Start()
    {
        UpdateArrow();
    }

    public void UpdateArrow()
    {
        // First priority:
        // Guide the player to the Aura Farm
        // if they haven't seen its introduction.
        if (!HasSeenCutscene())
        {
            arrow.SetTarget(target);
            return;
        }

        // After the introduction, only show the arrow
        // when the Aura Farm is full.
        if (IsAuraFull())
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

    bool IsAuraFull()
    {
        if (PlayerDataManager.Instance == null)
            return false;

        // Get Aura Farm level
        int level =
            PlayerDataManager.Instance.data.auraLevel;

        level =
            Mathf.Clamp(
                level,
                1,
                auraCapacityByLevel.Length
            );

        // Get capacity for current level
        float maxAura =
            auraCapacityByLevel[level - 1];

        // Calculate Aura per second
        float auraPerSecond =
            maxAura /
            (fillTimeHours * 60f * 60f);

        // Get saved timestamp
        long savedTicks =
            PlayerDataManager.Instance.data.auraLastUpdate;

        if (savedTicks <= 0)
            return false;

        // Convert timestamp
        DateTime lastUpdate =
            new DateTime(
                savedTicks,
                DateTimeKind.Utc
            );

        // Calculate elapsed time
        double elapsedSeconds =
            (
                DateTime.UtcNow -
                lastUpdate
            ).TotalSeconds;

        if (elapsedSeconds < 0)
            elapsedSeconds = 0;

        // Calculate current Aura
        float currentAura =
            (float)elapsedSeconds *
            auraPerSecond;

        // Don't exceed capacity
        currentAura =
            Mathf.Min(
                currentAura,
                maxAura
            );

        // Is it full?
        return currentAura >= maxAura;
    }
}