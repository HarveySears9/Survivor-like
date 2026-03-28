using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThudbuttTheMighty : Boss
{
    public GameObject hammerPrefab;
    public float hammerThrowCooldown = 5f;

    public float hammerDamage = 5;

    private float nextHammerThrowTime;

    private AnimateSprite animator;

    protected override void Start()
    {
        base.Start();
        // Initialize additional Grass Golem-specific logic
        nextHammerThrowTime = Time.time + hammerThrowCooldown;

        animator = GetComponent<AnimateSprite>();
    }

    protected override void Die()
    {
        base.Die(); // runs all the parent logic

        MissionManager.Instance.AddProgress($"kill_Thudbutt");
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        PerformThudbuttTheMightyAbilities();
    }

    private void PerformThudbuttTheMightyAbilities()
    {
        // Hammer Throw Attack
        if (Time.time >= nextHammerThrowTime)
        {
            if (animator.isMoving)
            {
                StartCoroutine(ThrowHammer());
                //nextHammerThrowTime = Time.time + hammerThrowCooldown;
            }
        }
    }

    private IEnumerator ThrowHammer()
    {
        animator.isMoving = false;
        currentSpeed /= 4; // Slow down while throwing the hammer

        if (hammerPrefab == null || playerTransform == null)
        {
            Debug.LogWarning("HammerPrefab or PlayerTransform is missing!");
            currentSpeed = originalSpeed;
            yield break;
        }

        yield return new WaitForSeconds(0.25f);

        // Capture the player's position at the time of the throw
        Vector2 targetPosition = playerTransform.position;

        // Instantiate the hammer
        animator.isMoving = false;
        GameObject hammer = Instantiate(hammerPrefab, transform.position, Quaternion.identity);

        // Initialize the hammer with the target position and Thudbutt's transform
        ThudbuttsHammer hammerScript = hammer.GetComponent<ThudbuttsHammer>();
        if (hammerScript != null)
        {
            hammerScript.Initialize(targetPosition, transform, this); // Send the hammer to the player's location
            hammerScript.damage = hammerDamage;
        }
    }

    public void HammerReturned()
    {
        // Reset Thudbutt's movement and start the cooldown
        animator.isMoving = true;
        currentSpeed = originalSpeed;

        // Start the cooldown timer here
        nextHammerThrowTime = Time.time + hammerThrowCooldown;
        Debug.Log("Hammer returned. Cooldown started.");
    }


}
