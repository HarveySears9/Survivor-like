using UnityEngine;

public class PenguinAOE : MonoBehaviour
{
    [Header("AOE")]
    public float radius = 2f;
    public float damage = 1f;

    [Header("Slow")]
    public float slowAmount = 0.4f;
    public float slowDuration = 2f;

    private bool hasAttacked = false;

    private void Start()
    {
        ApplyAOE();
    }

    private void ApplyAOE()
    {
        if (hasAttacked)
            return;

        hasAttacked = true;

        float scaledRadius = radius * transform.lossyScale.x;

        Collider2D[] targets = Physics2D.OverlapCircleAll(
            transform.position,
            scaledRadius
        );

        foreach (Collider2D target in targets)
        {
            if (!target.CompareTag("Enemy"))
                continue;

            EnemyController enemy =
                target.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                enemy.ApplySlow(slowAmount, slowDuration);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        float scaledRadius = radius * transform.lossyScale.x;

        Gizmos.DrawWireSphere(
            transform.position,
            scaledRadius
        );
    }
}