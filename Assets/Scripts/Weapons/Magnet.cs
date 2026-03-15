using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private List<GameObject> nearbyCollectibles = new List<GameObject>();  // Store nearby collectibles

    public LevelUpButtons levelUpButton;

    public int level = 0;
    public int maxLevel = 5;
    public float range = 0.5f;

    void Start()
    {
        levelUpButton.LevelUp(level, maxLevel);
    }
    
    public void LevelUp()
    {
        level++;
        if (level > maxLevel)
        {
            level = maxLevel;
        }
        switch (level)
        {
            case 1: range = 1f; break;
            case 2: range = 1.5f; break;
            case 3: range = 2f; break;
            case 4: range = 2.5f; break;
            case 5: range = 3f; break;
        }

        levelUpButton.LevelUp(level, maxLevel);
    }

    private void FixedUpdate()
    {
        AttractCollectibles();
    }

    private void AttractCollectibles()
    {
        // Get the magnet range based on the upgrade level

        // Find all collectibles within the range of the magnet
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Item"))
            {
                // Move the collectible towards the player
                Vector3 direction = (transform.position - collider.transform.position).normalized;
                collider.transform.position = Vector3.MoveTowards(collider.transform.position, transform.position, 5f * Time.deltaTime);
            }
        }
    }
}