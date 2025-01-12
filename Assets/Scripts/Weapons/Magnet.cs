using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private List<GameObject> nearbyCollectibles = new List<GameObject>();  // Store nearby collectibles

    public LevelUpButtons levelUpButton;

    public int level = 0;
    public int maxLevel = 5;
    public float range = 0;

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
        range = level/2f;

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