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

    protected override void Die()
    {
        base.Die(); // runs all the parent logic

        // Kill the orbiting staff
        if (activeStaffOrbit != null)
        {
            if (activeStaffOrbit != null && activeStaffOrbit.gameObject != null)
            {
                Destroy(activeStaffOrbit.gameObject);
            }
        }

        MissionManager.Instance.AddProgress($"kill_SnakeWoman");
    }
}