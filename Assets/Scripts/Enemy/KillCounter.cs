using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    public static int enemyKills = 0;
    public static int bossKills = 0;
    public TextMeshProUGUI killText;

    void OnEnable()
    {
        EnemyDeathEventManager.OnEnemyDeath += CountEnemyKill;
        EnemyDeathEventManager.OnBossDeath += CountBossKill;
    }

    void OnDisable()
    {
        EnemyDeathEventManager.OnEnemyDeath -= CountEnemyKill;
        EnemyDeathEventManager.OnBossDeath -= CountBossKill;
    }

    private void CountEnemyKill(Vector3 position)
    {
        enemyKills++;
        killText.text = "Kills: " + enemyKills.ToString();
    }

    private void CountBossKill(Vector3 position, GameObject[] dropPrefab)
    {
        bossKills++;
    }

    public static void ResetCounters()
    {
        enemyKills = 0;
        bossKills = 0;
    }
}
