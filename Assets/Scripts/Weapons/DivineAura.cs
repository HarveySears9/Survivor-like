using UnityEngine;

public class DivineAura : MonoBehaviour
{
    public float healAmount = 0f;           // Amount of HP to heal per tick
    public float cooldownDuration = 5f;    // Time in seconds between heals
    private float cooldownTimer;           // Tracks time since the last heal

    public GameObject gfx;

    public int level = 0;
    public int maxLevel = 5;

    public PlayerController player;       // Reference to the PlayerController script

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("PlayerController script not found on GameObject!");
        }
        cooldownTimer = cooldownDuration; // Initialize the cooldown timer
    }

    public void LevelUp()
    {
        level++;

        if (level > 0)
        {
            gfx.SetActive(true);
        }

        if (level > maxLevel)
        {
            level = maxLevel;
        }
        healAmount++;
    }

    void Update()
    {
        if (player != null && player.hp < player.maxHP)
        {
            cooldownTimer += Time.deltaTime;

            // If the cooldown period has passed, heal the player
            if (cooldownTimer >= cooldownDuration)
            {
                HealPlayer();
                cooldownTimer = 0f; // Reset the cooldown timer
            }
        }
    }

    void HealPlayer()
    {
        player.Heal(healAmount, true);
    }
}
