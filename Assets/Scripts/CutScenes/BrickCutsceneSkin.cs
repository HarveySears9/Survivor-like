using UnityEngine;

public class BrickCutsceneSkin : MonoBehaviour
{
    private SkinManager skinManager;
    public AnimateSprite animateSprite;

    void Start()
    {
        skinManager = FindObjectOfType<SkinManager>();

        if (skinManager == null)
        {
            Debug.LogError("BrickCutsceneSkin: SkinManager not found!");
            return;
        }

        skinManager.brickSp = animateSprite;

        skinManager.SetBrickSkin(
            PlayerDataManager.Instance.data.brickSkinEquipped
        );
    }
}