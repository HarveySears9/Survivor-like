using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public GameObject hammerPrefab;   // Hammer projectile
    public float fireRate = 2f;
    public int level = 0;
    public int maxLevel = 5;
    public float range = 10f;

    private float nextFireTime = 0f;

    public AnimateSprite playerAnimator;
    public AnimateImage levelUpButtonAnimator;

    public Sprite[] hammer1, hammer1moving, hammer2, hammer2moving, hammer3, hammer3moving, hammer4, hammer4moving, hammer5, hammer5moving;

    public LevelUpButtons levelUpButton;

    public SpriteRenderer sr;
    public PlayerController player;

    private float damage;

    private bool hammerActive = false;

    public bool unlocked = false;

    void Start()
    {
        if (unlocked)
        {
            SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();

            // Only enable for B'Rick (same as your sword logic)
            if (loadedData.currentCharacter != 0)
            {
                gameObject.SetActive(false);
                return;
            }

            damage = loadedData.currentDamage;

            levelUpButton.LevelUp(level, maxLevel);
            UpdateSprites();
        }
    }

    void Update()
    {
        if (level > 0)
        {
            if (Time.time >= nextFireTime)
            {
                if (!hammerActive)
                {
                    ThrowAtTargets();
                }
            }
        }

        playerAnimator.isMoving = player.isMoving;

        // Flip sprite
        if (player.moveDirection.x < 0)
            sr.flipX = true;
        else if (player.moveDirection.x > 0)
            sr.flipX = false;
    }

    void ThrowAtTargets()
    {
        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, range);

        List<Transform> enemyTargets = new List<Transform>();

        foreach (var col in targetsInRange)
        {
            if (col.CompareTag("Enemy"))
            {
                enemyTargets.Add(col.transform);
            }
        }

        if (enemyTargets.Count == 0) return;

        Transform target = enemyTargets[0]; // just pick first (or closest later)

        GameObject hammerObj = Instantiate(hammerPrefab, transform.position, Quaternion.identity);

        PlayerHammer hammer = hammerObj.GetComponent<PlayerHammer>();

        if (hammer != null)
        {
            hammer.Initialize(target.position, transform, this);
            hammer.damage = player.ApplyDamageModifiers(damage);
        }

        hammerActive = true;
        sr.enabled = false; // hide weapon sprite while thrown
    }

    public void LevelUp()
    {
        level++;

        if (level > maxLevel)
            level = maxLevel;

        levelUpButton.LevelUp(level, maxLevel);

        UpdateSprites();
    }

    void UpdateSprites()
    {
        switch (level)
        {
            case 1:
                playerAnimator.spriteArray = hammer1;
                playerAnimator.moveArray = hammer1moving;
                //levelUpButtonAnimator.spriteArray = hammer2;
                break;

            case 2:
                playerAnimator.spriteArray = hammer2;
                playerAnimator.moveArray = hammer2moving;
                //levelUpButtonAnimator.spriteArray = hammer3;
                break;

            case 3:
                playerAnimator.spriteArray = hammer3;
                playerAnimator.moveArray = hammer3moving;
                //levelUpButtonAnimator.spriteArray = hammer4;
                break;

            case 4:
                playerAnimator.spriteArray = hammer4;
                playerAnimator.moveArray = hammer4moving;
                //levelUpButtonAnimator.spriteArray = hammer5;
                break;

            case 5:
                playerAnimator.spriteArray = hammer5;
                playerAnimator.moveArray = hammer5moving;
                break;
        }
    }

    public void HammerReturned()
    {
        hammerActive = false;
        sr.enabled = true;

        float effectiveFireRate = fireRate * player.attackSpeedMultiplier;
        nextFireTime = Time.time + 1f / effectiveFireRate;
    }
}
