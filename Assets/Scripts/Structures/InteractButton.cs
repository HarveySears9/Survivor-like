using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractButton : MonoBehaviour
{
    private int index;

    public DragonAlterMenu dragonAlterMenu;

    public void Interact()
    {
        switch(index)
        {
            case 0:
                dragonAlterMenu.Open();
                this.gameObject.SetActive(false);
                break;
            default: 
                break;
        }
    }

    public void SetIndex(int index)
    {
        index = this.index;
    }
}
