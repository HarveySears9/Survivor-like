using UnityEngine;

public class AnimateHitbox : MonoBehaviour
{
    public AnimateSprite spriteAnimation;
    public Collider2D[] hitboxes;

    private int lastIndex = -1;

    void Start()
    {
        if (spriteAnimation == null)
            spriteAnimation = GetComponent<AnimateSprite>();

        DisableAll();

        UpdateHitbox();
    }

    void Update()
    {
        if (spriteAnimation == null)
            return;

        if (spriteAnimation.CurrentIndex != lastIndex)
        {
            UpdateHitbox();
        }
    }

    void UpdateHitbox()
    {
        int index = spriteAnimation.CurrentIndex;

        lastIndex = index;

        DisableAll();

        if (index >= 0 && index < hitboxes.Length)
        {
            hitboxes[index].enabled = true;
        }
    }

    void DisableAll()
    {
        foreach (Collider2D hitbox in hitboxes)
        {
            if (hitbox != null)
                hitbox.enabled = false;
        }
    }
}