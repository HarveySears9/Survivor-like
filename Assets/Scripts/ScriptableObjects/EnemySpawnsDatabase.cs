using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnsDatabase", menuName = "Game/EnemySpawnsDatabase")]
public class EnemySpawnsDatabase : ScriptableObject
{
    public EnemySpawns[] enemySpawns;
}
