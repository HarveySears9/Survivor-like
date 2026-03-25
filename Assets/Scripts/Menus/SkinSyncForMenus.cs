using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSyncForMenus : MonoBehaviour
{
    public AnimateSprite player;
    public AnimateImage menu;

    void OnEnable()
    {
        menu.spriteArray = player.spriteArray;
    }
}
