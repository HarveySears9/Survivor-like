using System.Collections;
using UnityEngine;

public class NPCs : MonoBehaviour
{
    private SpriteRenderer sr;

    public float flipRange = 5f; // Max time between flips

    public bool idle = true;

    private Transform player;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(FlipSprite()); // Start the coroutine
    }

    IEnumerator FlipSprite()
    {
        while (true) // Loop indefinitely
        {
            yield return new WaitForSeconds(Random.Range(2f, flipRange));
            if (idle)
            {
                sr.flipX = !sr.flipX; // Flip the sprite horizontally
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.transform;
        FacePlayer();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        idle = true;
        player = null;
    }

    private void FacePlayer()
    {
        if (player != null)
        {
            idle = false;
            float direction = player.position.x - transform.position.x;

            // Flip based on player's relative position
            sr.flipX = direction < 0;
        }
    }

}
