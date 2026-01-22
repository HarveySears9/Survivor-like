using UnityEngine;

public class DragonAlter : MonoBehaviour
{
    private AnimateSprite animator;
    public bool isActive = true;

    [SerializeField] private DragonDealTable dealTable;
    private DragonDeal assignedDeal;

    private InteractButton button;
    private DragonAlterMenu menu;

    private PlayerController player;

    void Awake()
    {
        animator = GetComponent<AnimateSprite>();

        // Find the menu at runtime
        menu = FindObjectOfType<DragonAlterMenu>(true);
        if (menu == null)
        {
            Debug.LogError("DragonAlterMenu not found in scene!");
        }
        button = FindObjectOfType<InteractButton>(true);

        player = FindObjectOfType<PlayerController>();
        AssignDeal();
    }

    void AssignDeal()
    {
        assignedDeal = dealTable.GetRandomDeal();
    }

    public DragonDeal GetDeal()
    {
        return assignedDeal;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (animator != null)
        {
            if (isActive)
            {
                animator.isMoving = true;
                button.gameObject.SetActive(true);
                button.SetIndex(0);
                menu.SetAlter(this);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (animator != null)
        {
            animator.isMoving = false;
            button.gameObject.SetActive(false);
            button.SetIndex(-1);
            menu.SetAlter(null);
        }
    }

    public void TakeDeal()
    {
        isActive = false;
        animator.StopAnimation();
        animator.spriteRenderer.sprite = animator.spriteArray[0];
        button.SetIndex(-1);

        player.TakeDamage(player.maxHP * assignedDeal.hpCostPercent);

        switch (assignedDeal.dealType)
        {
            case DragonDealType.MaxHPIncrease:
                player.IncreaseMaxHP(assignedDeal.value);
                break;

            case DragonDealType.GoldGain:
                player.IncreaseCoinMultiplyer(assignedDeal.value);
                break;

            case DragonDealType.FreeLevel:
                FindObjectOfType<EXPBar>().ScrollPickUp();
                break;
        }
    }
}
