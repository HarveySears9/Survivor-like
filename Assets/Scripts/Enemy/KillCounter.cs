using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public static int enemyKills = 0;
    public static int bossKills = 0;

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
