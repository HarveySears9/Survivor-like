using UnityEngine;

[CreateAssetMenu(menuName = "Dragon Altar/Deal Table")]
public class DragonDealTable : ScriptableObject
{
    public DragonDeal[] deals;

    public DragonDeal GetRandomDeal()
    {
        if (deals == null || deals.Length == 0)
            return null;

        int totalWeight = 0;
        foreach (var deal in deals)
            totalWeight += deal.weight;

        int roll = Random.Range(0, totalWeight);

        int current = 0;
        foreach (var deal in deals)
        {
            current += deal.weight;
            if (roll < current)
                return deal;
        }

        return deals[0]; // fallback (should never hit)
    }
}
