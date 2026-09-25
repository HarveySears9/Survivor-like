using UnityEngine;

public class LavaPool : MonoBehaviour
{
    public float damage = 1f;
    public float damageInterval = 1f;
    public float lifetime = 4f;

    private float lastDamageTime = 0f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageInterval)
            {
                lastDamageTime = Time.time;

                PlayerController player =
                    other.GetComponent<PlayerController>();

                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }
        }
    }
}