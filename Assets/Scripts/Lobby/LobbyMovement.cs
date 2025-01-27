using UnityEngine;

public class LobbyMovement : MonoBehaviour
{
    public GameObject[] characters; // Array of characters
    public float speed;             // Movement speed
    private AnimateSprite animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDirection = Vector2.zero;
    public VariableJoystick variableJoystick;
    private Rigidbody2D rb;         // Rigidbody2D for physics-based movement

    void Start()
    {
        // Activate the correct character based on saved data
        int characterIndex = SaveFile.LoadData<SaveFile.Data>().currentCharacter;
        characters[characterIndex].SetActive(true);
        animator = characters[characterIndex].GetComponent<AnimateSprite>();
        spriteRenderer = characters[characterIndex].GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); // Ensure the Rigidbody2D is attached to the parent GameObject
    }

    void FixedUpdate()
    {
        // Get movement input from joystick
        moveDirection = new Vector2(variableJoystick.Horizontal, variableJoystick.Vertical);

        // Apply movement using Rigidbody2D
        rb.velocity = moveDirection * speed;

        // Handle animation and sprite flipping
        if (moveDirection != Vector2.zero)
        {
            animator.isMoving = true;

            // Flip sprite based on horizontal movement direction
            spriteRenderer.flipX = moveDirection.x < 0;
        }
        else
        {
            animator.isMoving = false;
        }
    }
}
