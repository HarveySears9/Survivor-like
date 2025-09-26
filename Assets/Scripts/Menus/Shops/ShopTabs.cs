using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTabs : MonoBehaviour
{
    public Color defaultColour;
    public Color highlightedColour;

    public Image[] tabs;

    public void OnTabPressed(int index)
    {
        foreach (var tab in tabs)
        {
            tab.color = defaultColour;
        }
        tabs[index].color = highlightedColour;
    }
}
