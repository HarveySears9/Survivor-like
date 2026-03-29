using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public SpriteRenderer sr;

    public AnimateSprite playerAnimator;
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerAnimator.isMoving = player.isMoving;

        // Flip the sprite's X-axis based on the player's movement direction
        if (player.moveDirection.x < 0)
        {
            sr.flipX = true; // Flip sprite when moving left
        }
        else if (player.moveDirection.x > 0)
        {
            sr.flipX = false; // Keep sprite normal when moving right
        }
    }
    /*
    public void LevelUp()
    {
        level++;
        if (level > maxLevel)
        {
            level = maxLevel;
        }

        levelUpButton.LevelUp(level, maxLevel);

        UpdateSprites();
    } */
}
