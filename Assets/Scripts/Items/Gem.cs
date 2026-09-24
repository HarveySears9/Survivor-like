using UnityEngine;

public class Gem : MonoBehaviour
{
    public int value = 1;
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

        EXPBar playerEXP = other.GetComponent<EXPBar>();

        if (playerEXP != null)
        {
            playerEXP.AddEXP(value);
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