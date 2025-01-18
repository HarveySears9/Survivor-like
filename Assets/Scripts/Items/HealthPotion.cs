using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public float healPercentage = 10f;

    public float lifetime = 30f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Health potion aquired");
            // Handle player damage logic here
            other.GetComponent<PlayerController>().Heal(healPercentage, false);
            Destroy(gameObject);
        }
    }
}
