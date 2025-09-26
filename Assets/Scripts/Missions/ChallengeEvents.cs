using System;

public static class ChallengeEvents
{
    public static event Action<EnemyType> OnEnemyDefeated;
    public static event Action OnBossDefeated;
    public static event Action<float> OnSurvivalTime;

    public static void EnemyDefeated(EnemyType type)
    {
        OnEnemyDefeated?.Invoke(type);
        if (type == EnemyType.Boss)
            OnBossDefeated?.Invoke();
    }

    public static void SurvivalTime(float seconds)
    {
        OnSurvivalTime?.Invoke(seconds);
    }
}
