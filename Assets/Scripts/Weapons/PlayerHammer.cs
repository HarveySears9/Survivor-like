using System.Collections;
using UnityEngine;

public class PlayerHammer : MonoBehaviour
{
    public float speed = 6f;
    public float damage = 5f;

    private Vector2 targetPosition;
    private Transform playerTransform;
    private bool returning = false;

    public float returnDelay = 0.5f;

    private Vector2 direction;

    public GameObject gfx;
    public float spinSpeed = 500f;
    private Hammer owner;

    public void Initialize(Vector2 targetPosition, Transform playerTransform, Hammer owner)
    {
        this.targetPosition = targetPosition;
        this.playerTransform = playerTransform;
        this.owner = owner;

        direction = (targetPosition - (Vector2)transform.position).normalized;

        StartCoroutine(ReturnHammer());
    }

    void Update()
    {
        if (returning)
        {
            direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        }

        transform.Translate(direction * speed * Time.deltaTime);

        // Return complete
        if (returning && Vector2.Distance(transform.position, playerTransform.position) < 0.2f)
        {
            owner.HammerReturned();
            Destroy(gameObject);
        }
    }

    IEnumerator ReturnHammer()
    {
        yield return new WaitForSeconds(returnDelay);
        returning = true;
    }

    void FixedUpdate()
    {
        gfx.transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }
}