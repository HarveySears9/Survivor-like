using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 5f;      // Speed of the fireball
    public float lifetime = 3f;  // Time before the fireball disappears

    private Vector2 moveDirection;

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
}
