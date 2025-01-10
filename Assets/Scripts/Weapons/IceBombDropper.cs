using UnityEngine;

public class IceBombDropper : MonoBehaviour
{
    public GameObject iceBombPrefab;  // Prefab for the ice bomb
    public float dropInterval = 5f;  // Time interval between drops
    public int level;

    private float nextDropTime = 0f; // Tracks when the next bomb can be dropped

    void Start()
    {
        level = 1;
    }

    void Update()
    {
        // Check if it's time to drop the next bomb
        if (Time.time >= nextDropTime)
        {
            DropIceBomb();
            nextDropTime = Time.time + dropInterval; // Set the time for the next drop
        }
    }

    void DropIceBomb()
    {
        // Instantiate the ice bomb at the player's current position
        Instantiate(iceBombPrefab, transform.position, Quaternion.identity);
    }
}
