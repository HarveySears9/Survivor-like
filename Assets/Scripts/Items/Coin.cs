using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;

    public float lifetime = 30f;

    public AudioClip pickupSound;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().AddCoin(value);

            AudioManager.Instance.PlaySFX(pickupSound);

            Destroy(gameObject);
        }
    }
}
