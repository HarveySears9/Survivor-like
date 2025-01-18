using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
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
            Debug.Log("Coin aquired");
            Destroy(gameObject);
        }
    }
}
