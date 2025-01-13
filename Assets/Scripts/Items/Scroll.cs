using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EXPBar playerEXP = other.GetComponent<EXPBar>();
            if (playerEXP != null)
            {
                playerEXP.ScrollPickUp(); // Add EXP to the player
            }

            // Destroy the gem after it's collected
            Destroy(gameObject);
        }
    }
}