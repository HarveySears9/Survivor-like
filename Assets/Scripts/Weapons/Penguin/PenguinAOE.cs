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

    // Reusable buffer — no allocation every AOE
    private Collider2D[] targets = new Collider2D[64];

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

        int count = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            scaledRadius,
            targets
        );

        for (int i = 0; i < count; i++)
        {
            Collider2D target = targets[i];

            if (target == null || !target.CompareTag("Enemy"))
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