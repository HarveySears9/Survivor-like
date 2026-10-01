using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordSlash : WeaponBase
{
    [Header("Sword Slash")]
    public GameObject slashPrefab;
    public float range = 10f;

    [Header("Player Animation")]
    public AnimateSprite playerAnimator;
    public AnimateImage levelUpButtonAnimator;

    public Sprite[] sword1, sword1moving;
    public Sprite[] sword2, sword2moving;
    public Sprite[] sword3, sword3moving;
    public Sprite[] sword4, sword4moving;
    public Sprite[] sword5, sword5moving;

    public LevelUpButtons levelUpButton;

    public SpriteRenderer sr;


    protected override void Start()
    {
        SaveFile.Data loadedData =
            SaveFile.LoadData<SaveFile.Data>();

        // Sword Slash is only available to B'Rick.
        if (loadedData.currentCharacter != 0)
        {
            gameObject.SetActive(false);
            return;
        }

        base.Start();

        level = 0;

        levelUpButton.LevelUp(level, maxLevel);

        UpdateSprites();
    }


    void Update()
    {
        if (level <= 0)
            return;

        if (CooldownReady())
        {
            StartCoroutine(FireAtTargets());
        }

        playerAnimator.isMoving = player.isMoving;

        // Flip the sword sprite based on movement direction.
        if (player.moveDirection.x < 0)
        {
            sr.flipX = true;
        }
        else if (player.moveDirection.x > 0)
        {
            sr.flipX = false;
        }

        UpdateCooldownUI();
    }


    IEnumerator FireAtTargets()
    {
        Collider2D[] targetsInRange =
            Physics2D.OverlapCircleAll(
                transform.position,
                range
            );

        List<Transform> enemyTargets =
            new List<Transform>();

        foreach (var target in targetsInRange)
        {
            if (target.CompareTag("Enemy"))
            {
                enemyTargets.Add(target.transform);
            }
        }

        // No targets, so don't start the cooldown.
        if (enemyTargets.Count == 0)
            yield break;

        // Start cooldown now that we know we are actually attacking.
        StartCooldown();

        for (int i = 0; i < level; i++)
        {
            Transform target =
                enemyTargets[i % enemyTargets.Count];

            Vector2 fireDirection =
                (target.position - transform.position)
                .normalized;

            GameObject slash =
                Instantiate(
                    slashPrefab,
                    transform.position,
                    Quaternion.identity
                );

            // Add a small random angle variation.
            float randomOffset =
                Random.Range(-5f, 5f);

            float angleToRotate =
                Mathf.Atan2(
                    fireDirection.y,
                    fireDirection.x
                ) * Mathf.Rad2Deg + randomOffset;

            slash.transform.rotation =
                Quaternion.Euler(
                    new Vector3(
                        0,
                        0,
                        angleToRotate
                    )
                );

            slash.GetComponent<Fireball>()
                .Initialize(Vector2.right);

            slash.GetComponent<Weapon>().damage =
                GetWeaponDamage();

            // Tiny delay between slashes.
            yield return new WaitForSeconds(0.1f);
        }
    }


    public override void LevelUp()
    {
        level++;

        if (level > maxLevel)
            level = maxLevel;

        if (level == 1)
        {
            CreateWeaponUI();
        }

        levelUpButton.LevelUp(level, maxLevel);

        UpdateSprites();
    }


    void UpdateSprites()
    {
        switch (level)
        {
            case 1:
                playerAnimator.spriteArray = sword1;
                playerAnimator.moveArray = sword1moving;
                levelUpButtonAnimator.spriteArray = sword2;
                break;

            case 2:
                playerAnimator.spriteArray = sword2;
                playerAnimator.moveArray = sword2moving;
                levelUpButtonAnimator.spriteArray = sword3;
                break;

            case 3:
                playerAnimator.spriteArray = sword3;
                playerAnimator.moveArray = sword3moving;
                levelUpButtonAnimator.spriteArray = sword4;
                break;

            case 4:
                playerAnimator.spriteArray = sword4;
                playerAnimator.moveArray = sword4moving;
                levelUpButtonAnimator.spriteArray = sword5;
                break;

            case 5:
                playerAnimator.spriteArray = sword5;
                playerAnimator.moveArray = sword5moving;
                break;
        }
    }
}