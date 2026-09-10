using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    public void UpdateCoins()
    {
        coinText.text = $": {PlayerDataManager.Instance.data.coins}";
    }
}
