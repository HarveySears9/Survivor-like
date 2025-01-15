using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public float speed = 5f;      // Speed of the fireball
    public float lifetime = 3f;  // Time before the fireball disappears
    public float damage = 1f;

    private Vector2 moveDirection;

    public GameObject gfx;
    public float spinSpeed = 5f;

    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized;
        Destroy(gameObject, lifetime); // Destroy after the lifetime expires
    }

    void Update()
    {
        // Move the fireball in the set direction
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        // Rotate the main object (spinning around the Z-axis)
        gfx.transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime); // Spin around Z-axis
    }

        void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy hit the player!");
            // Handle player damage logic here
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
