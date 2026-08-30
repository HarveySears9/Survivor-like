using System;
using UnityEngine;

public class MontgomeryAuraVisual : MonoBehaviour
{
    [Header("Aura Sprite")]
    public SpriteRenderer sr;

    [Header("Aura Settings")]
    public float fillTimeHours = 12f;

    // Same capacities as the Aura Farm
    private readonly float[] auraCapacityByLevel =
    {
        1000f,  // Level 1
        1500f,  // Level 2
        2250f   // Level 3
    };


    void Start()
    {
        UpdateAuraVisual();
    } 


    public void UpdateAuraVisual()
    {
        if (PlayerDataManager.Instance == null)
            return;


        // Get Aura Farm level
        int level =
            PlayerDataManager.Instance.data.auraLevel;

        level =
            Mathf.Clamp(
                level,
                1,
                auraCapacityByLevel.Length
            );


        // Get this level's capacity
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
        {
            SetAuraAlpha(0f);
            return;
        }


        // Convert saved ticks back into a DateTime
        DateTime lastUpdate =
            new DateTime(
                savedTicks,
                DateTimeKind.Utc
            );


        // Work out how long the farm has been generating
        double elapsedSeconds =
            (DateTime.UtcNow - lastUpdate).TotalSeconds;


        if (elapsedSeconds < 0)
            elapsedSeconds = 0;


        // Calculate how much Aura has been generated
        float currentAura =
            (float)elapsedSeconds * auraPerSecond;


        // Don't allow it to go above capacity
        currentAura =
            Mathf.Min(
                currentAura,
                maxAura
            );


        // Convert to percentage
        float percentage =
            Mathf.Clamp01(
                currentAura / maxAura
            );


        // Update sprite
        SetAuraAlpha(percentage);
    }


    void SetAuraAlpha(float percentage)
    {
        // 0% = invisible
        // 100% = 65% opacity
        float alpha =
            Mathf.Lerp(
                0f,
                0.65f,
                percentage
            );


        Color spriteColor = sr.color;

        spriteColor.a = alpha;

        sr.color = spriteColor;
    }
}