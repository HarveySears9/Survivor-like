using UnityEngine;

public class AnimateSprite : MonoBehaviour
{
    public Sprite[] spriteArray;
    public Sprite[] moveArray;
    public SpriteRenderer spriteRenderer;

    public bool animating;
    public float animationSpeed = 0.25f;

    public bool isMoving = false;
    public bool destroyOnFinish = false;

    public AnimateSprite masterSprite;

    private int currentIndex = 0;
    private bool lastMovingState;

    public int CurrentIndex => currentIndex;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        lastMovingState = isMoving;

        // Only start an animation timer if we don't
        // have a master controlling our frame.
        if (masterSprite == null && animating)
        {
            StartAnimation();
        }

        UpdateSprite();
    }

    void Update()
    {
        if (!animating || spriteRenderer == null)
            return;

        // If our movement state changed, reset to frame 0.
        if (isMoving != lastMovingState)
        {
            lastMovingState = isMoving;
            currentIndex = 0;

            UpdateSprite();
        }

        // Master controls our animation frame.
        if (masterSprite != null)
        {
            if (!masterSprite.animating)
                return;

            int masterIndex = masterSprite.CurrentIndex;

            if (currentIndex != masterIndex)
            {
                currentIndex = masterIndex;
                UpdateSprite();
            }
        }
    }

    public void StartAnimation()
    {
        animating = true;

        // A master controls the animation timing,
        // so we don't need InvokeRepeating.
        if (masterSprite != null)
            return;

        CancelInvoke(nameof(AdvanceAnimation));

        InvokeRepeating(
            nameof(AdvanceAnimation),
            0f,
            animationSpeed
        );
    }

    public void StopAnimation()
    {
        animating = false;

        CancelInvoke(nameof(AdvanceAnimation));
    }

    private void AdvanceAnimation()
    {
        if (!animating || spriteRenderer == null)
            return;

        Sprite[] activeArray =
            isMoving ? moveArray : spriteArray;

        if (activeArray == null || activeArray.Length == 0)
            return;

        currentIndex++;

        if (currentIndex >= activeArray.Length)
        {
            currentIndex = 0;

            if (destroyOnFinish)
            {
                Destroy(gameObject);
                return;
            }
        }

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        Sprite[] activeArray =
            isMoving ? moveArray : spriteArray;

        if (activeArray == null || activeArray.Length == 0)
            return;

        // If the moving/idle arrays have different lengths,
        // prevent an invalid index.
        if (currentIndex >= activeArray.Length)
            currentIndex = 0;

        spriteRenderer.sprite =
            activeArray[currentIndex];
    }
}