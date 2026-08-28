using UnityEngine;

public class Montgomery : MonoBehaviour
{
    [Header("Montgomery Sprite Animation")]
    public AnimateSprite animateSprite;

    [Header("Sprites By Level")]
    public Sprite[] level1Sprites;
    public Sprite[] level2Sprites;
    public Sprite[] level3Sprites;


    void Start()
    {
        UpdateArray();
    }


    public void UpdateArray()
    {
        int level = PlayerDataManager.Instance.data.auraLevel;

        switch (level)
        {
            case 1:
                animateSprite.spriteArray = level1Sprites;
                break;

            case 2:
                animateSprite.spriteArray = level2Sprites;
                break;

            case 3:
                animateSprite.spriteArray = level3Sprites;
                break;

            default:
                Debug.LogWarning("Invalid Montgomery level: " + level);
                animateSprite.spriteArray = level1Sprites;
                break;
        }
    }
}