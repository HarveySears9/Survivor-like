using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; // For pointer events

public class PlayerController : MonoBehaviour
{
    public int maxHP = 10;
    public float hp;
    public float speed;
    public VariableJoystick variableJoystick;
    [SerializeField] private FireBreath fireBreath;
    private AnimateSprite animator;
    private SpriteRenderer spriteRenderer; // To control flipping of the sprite

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
        animator = GetComponent<AnimateSprite>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // For 2D movement (X and Y only)
        Vector2 moveDirection = new Vector2(variableJoystick.Horizontal, variableJoystick.Vertical);

        // Move the player in the direction specified by moveDirection
        transform.Translate(moveDirection * speed * Time.deltaTime);

        if (moveDirection != Vector2.zero)
        {
            fireBreath.moveDirection = moveDirection;
            animator.isMoving = true;

            // Flip sprite based on movement direction
            if (moveDirection.x < 0)
            {
                spriteRenderer.flipX = true; // Flip sprite when moving left
            }
            else if (moveDirection.x > 0)
            {
                spriteRenderer.flipX = false; // Keep sprite normal when moving right
            }
        }
        else
        {
            animator.isMoving = false;
        }

    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(this);
        }
    }

    public void Heal(float percentage)
    {
        hp += (percentage/100f)*maxHP;
        if (hp > maxHP)
        {
            hp = maxHP;
        }
    }
}