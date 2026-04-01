using UnityEngine;
using TMPro;

public class DragonAlterMenu : MonoBehaviour
{
    private DragonAlter currentAlter;

    private DragonDeal deal = null;

    private bool openLevelUpMenu = false;

    [SerializeField] private TextMeshProUGUI costText, rewardText, rewardAmount, dealFlavourText;

    public void SetAlter(DragonAlter alter) 
    { 
        currentAlter = alter;
        SetUpDeal();
    }

    public void Open()
    {
        Time.timeScale = 0f;
        this.gameObject.SetActive(true);
        openLevelUpMenu = false;
    }

    public void Close()
    {
        if(!openLevelUpMenu)
        {
            Time.timeScale = 1f;
        }
        currentAlter = null;
        this.gameObject.SetActive(false);
    }

    public void AcceptDeal()
    {
        if (currentAlter == null) return;

        if (deal.dealType == DragonDealType.FreeLevel)
        {
            openLevelUpMenu = true;
        }

        currentAlter.TakeDeal();
        Close();
    }

    private void SetUpDeal()
    {
        if (currentAlter == null)
            return;

        deal = currentAlter.GetDeal();

        float hpCostPercent = deal.hpCostPercent * 100f;
        costText.text = $"-{hpCostPercent:0}% HP";

        switch (deal.dealType)
        {
            case DragonDealType.MaxHPIncrease:
                dealFlavourText.text = "The dragon hardens your flesh.";
                rewardText.text = "Max HP";
                rewardAmount.text = $"+{deal.value * 100f:0}%";
                break;

            //case DragonDealType.GoldGain:
            //rewardText.text = "Gold Gain";
            //rewardAmount.text = $"+{deal.value * 100f:0}%";
            //break;

            case DragonDealType.AttackSpeed:
                dealFlavourText.text = "Your blood moves faster.";
                rewardText.text = "Attack Speed";
                rewardAmount.text = $"+{deal.value * 100f:0}%";
                break;

            case DragonDealType.RageBoost:
                dealFlavourText.text = "Pain sharpens your strikes.";
                rewardText.text = "Attack When\nHP is low";
                rewardAmount.text = $"+{deal.value * 100f:0}%";
                break;

            case DragonDealType.Lifesteal:
                dealFlavourText.text = "The dragon feeds through you.";
                rewardText.text = "Life Steal";
                rewardAmount.text = $"+{deal.value * 100f:0}%";
                break;

            case DragonDealType.FreeLevel:
                dealFlavourText.text = "The dragon accelerates your growth.";
                rewardText.text = "Level";
                rewardAmount.text = $"+{deal.value:0}";
                break;
        }
    }

}
