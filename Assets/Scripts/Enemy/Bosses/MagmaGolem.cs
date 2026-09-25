using System.Collections;
using UnityEngine;

public class MagmaGolem : Boss
{
    public GameObject magmaBoulderPrefab;

    public float boulderCooldown = 4f;
    public int boulderCount = 3;
    public float spreadAngle = 30f;
    public float boulderDamage = 1.5f;

    private float nextBoulderTime;

    protected override void Start()
    {
        base.Start();
        nextBoulderTime = Time.time + boulderCooldown;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        PerformMagmaGolemAbilities();
    }

    private void PerformMagmaGolemAbilities()
    {
        if (Time.time >= nextBoulderTime)
        {
            StartCoroutine(ThrowMagmaBoulders());
            nextBoulderTime = Time.time + boulderCooldown;
        }
    }

    private IEnumerator ThrowMagmaBoulders()
    {
        moving = false;

        yield return new WaitForSeconds(0.25f);

        if (magmaBoulderPrefab == null || playerTransform == null)
        {
            moving = true;
            yield break;
        }

        float angleStep = spreadAngle / (boulderCount - 1);
        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < boulderCount; i++)
        {
            float angle = startAngle + i * angleStep;

            Vector2 fireDirection =
                Quaternion.Euler(0, 0, angle) *
                (playerTransform.position - transform.position).normalized;

            GameObject magmaBoulder =
                Instantiate(
                    magmaBoulderPrefab,
                    transform.position,
                    Quaternion.identity
                );

            float angleToRotate =
                Mathf.Atan2(
                    fireDirection.y,
                    fireDirection.x
                ) * Mathf.Rad2Deg;

            magmaBoulder.transform.rotation =
                Quaternion.Euler(0, 0, angleToRotate);

            Boulder boulderScript =
                magmaBoulder.GetComponent<Boulder>();

            if (boulderScript != null)
            {
                boulderScript.Initialize(Vector2.right);
                boulderScript.damage = boulderDamage;
            }

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);

        moving = true;
    }
}