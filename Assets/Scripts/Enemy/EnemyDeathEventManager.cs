using UnityEngine;

public class EnemyDeathEventManager : MonoBehaviour
{
    // Define a delegate and event for enemy death
    public delegate void EnemyDeath(Vector3 position);
    public static event EnemyDeath OnEnemyDeath;

    public delegate void BossDeath(Vector3 position);
    public static event BossDeath OnBossDeath;

    // Method to invoke the event
    public static void EnemyDied(Vector3 position)
    {
        OnEnemyDeath?.Invoke(position);
    }

    // Method to invoke the event
    public static void BossDied(Vector3 position)
    {
        OnBossDeath?.Invoke(position);
    }
}
