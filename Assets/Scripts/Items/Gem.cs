using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int value = 1;

    public float lifetime = 30f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EXPBar playerEXP = other.GetComponent<EXPBar>();
            if (playerEXP != null)
            {
                playerEXP.AddEXP(value); // Add EXP to the player
            }

            // Destroy the gem after it's collected
            Destroy(gameObject);
        }
    }
}
