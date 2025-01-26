using System.Collections;
using UnityEngine;

public class ThudbuttsHammer : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 5f;
    private Vector2 targetPosition;
    private Transform thudbuttTransform;
    private bool returning = false;
    private ThudbuttTheMighty thudbuttScript;
    public float returnDelay = 1f;

    private Vector2 currentTarget;
    private Vector2 direction;

    public GameObject gfx;
    public float spinSpeed = 500f;

    public void Initialize(Vector2 targetPosition, Transform thudbuttTransform, ThudbuttTheMighty thudbuttScript)
    {
        this.targetPosition = targetPosition;
        this.thudbuttTransform = thudbuttTransform;
        this.thudbuttScript = thudbuttScript;

        currentTarget = targetPosition;
        direction = (currentTarget - (Vector2)transform.position).normalized;

        StartCoroutine(ReturnHammer());
    }

    void Update()
    {
        // Move towards the target position or back to Thudbutt
        if (returning)
        {
            currentTarget = thudbuttTransform.position;
            direction = (currentTarget - (Vector2)transform.position).normalized;
        }

        // Move the hammer
        transform.Translate(direction * speed * Time.deltaTime);

        // Check if the hammer reached its destination
        if (Vector2.Distance(transform.position, thudbuttTransform.position) < 0.1f && returning)
        {
            // Destroy the hammer and notify Thudbutt
            thudbuttScript.HammerReturned();
            Destroy(gameObject);
        
        }
    }

    private IEnumerator ReturnHammer()
    {
        yield return new WaitForSeconds(returnDelay);
        returning = true;
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
            Debug.Log("Hammer hit the player!");
            other.GetComponent<PlayerController>()?.TakeDamage(damage);
        }
    }
}
