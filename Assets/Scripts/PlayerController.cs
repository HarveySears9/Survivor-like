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

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
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
}