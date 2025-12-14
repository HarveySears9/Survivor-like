using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeWomanBoss : Boss
{
    public GameObject staffPrefab;
    public float staffCooldown = 6f;
    public float staffDuration = 3f;

    private float nextStaffTime;
    private AnimateSprite animator;

    private SnakeStaffOrbit activeStaffOrbit;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<AnimateSprite>();
        nextStaffTime = Time.time + staffCooldown;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Time.time >= nextStaffTime)
        {
            StartCoroutine(StaffAttack());
            nextStaffTime = Time.time + staffCooldown;
        }
    }

    private IEnumerator StaffAttack()
    {
        animator.isMoving = false;
        currentSpeed = originalSpeed * 0.3f;

        GameObject staff = Instantiate(staffPrefab, transform.position, Quaternion.identity);
        SnakeStaffOrbit orbit = staff.GetComponent<SnakeStaffOrbit>();
        orbit.Initialize(this, staffDuration);
        SetStaffOrbit(orbit);

        yield return new WaitForSeconds(staffDuration);

        animator.isMoving = true;
        currentSpeed = originalSpeed;
    }

    public void SetStaffOrbit(SnakeStaffOrbit staff)
    {
        activeStaffOrbit = staff;
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);

        if (health <= 0f)
        {
            if (!isDead)
            {
                isDead = true;

                // Kill the orbiting staff
                if (activeStaffOrbit != null)
                {
                    if (activeStaffOrbit != null && activeStaffOrbit.gameObject != null)
                    {
                        Destroy(activeStaffOrbit.gameObject);
                    }
                }

                EnemyDeathEventManager.BossDied(transform.position, drops);
                MissionManager.Instance.AddProgress("kill_Boss");

                Destroy(gameObject);
            }
        }
    }
}