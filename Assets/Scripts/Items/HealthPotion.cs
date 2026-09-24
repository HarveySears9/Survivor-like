using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public float healPercentage = 10f;
    public float lifetime = 30f;
    public AudioClip pickupSound;

    private float lifetimeTimer;

    void OnEnable()
    {
        lifetimeTimer = lifetime;
    }

    void Update()
    {
        lifetimeTimer -= Time.deltaTime;

        if (lifetimeTimer <= 0f)
        {
            ReturnToPool();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerController player =
            other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.Heal(healPercentage, false);
        }

        AudioManager.Instance.PlaySFX(pickupSound);

        ReturnToPool();
    }

    void ReturnToPool()
    {
        if (ItemPool.Instance != null)
        {
            ItemPool.Instance.Return(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}