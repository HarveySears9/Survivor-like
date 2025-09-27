using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float fallSpeed = 10f;
    public float damage = 10f;

    public GameObject impactPrefab;
    public GameObject shadowPrefab;

    public Vector3 targetPosition;

    private GameObject shadowInstance;
    private float startY;
    private float elapsed = 0f;

    private Vector3 shadowStartScale;

    void Start()
    {
        // Spawn shadow at target
        Vector3 shadowPos = targetPosition;
        shadowPos.y -= 0.5f; // slight offset
        shadowInstance = Instantiate(shadowPrefab, shadowPos, Quaternion.identity);

        // Store the prefab's original scale
        shadowStartScale = shadowInstance.transform.localScale;

        // Start small
        shadowInstance.transform.localScale = shadowStartScale * 0.1f;

        startY = transform.position.y;
    }

    void Update()
    {
        // Move meteor down
        float step = fallSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // Scale shadow based on progress
        if (shadowInstance != null)
        {
            float progress = 1f - (transform.position.y - targetPosition.y) / (startY - targetPosition.y);
            shadowInstance.transform.localScale = Vector3.Lerp(shadowStartScale * 0.1f, shadowStartScale, progress);
        }

        // Check if reached target
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Impact();
        }
    }

    void Impact()
    {
        // Spawn the impact AOE at the meteor's current position
        Instantiate(impactPrefab, transform.position, Quaternion.identity);

        if (shadowInstance != null)
            Destroy(shadowInstance);

        // Destroy the meteor itself
        Destroy(gameObject);
    }
}
