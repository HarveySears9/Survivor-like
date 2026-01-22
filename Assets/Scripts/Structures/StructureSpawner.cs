using UnityEngine;

public class StructureSpawner : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] structures;

    [SerializeField] private WorldMessageUI worldMessage;
    [SerializeField] private ObjectiveArrow objectiveArrow;

    [SerializeField] private float spawnRadius = 8f;
    [SerializeField] private float minDistanceFromPlayer = 5f;

    void Awake()
    {
        gameTimer.OnSpawnStructure += SpawnStructure;
    }

    void OnDestroy()
    {
        gameTimer.OnSpawnStructure -= SpawnStructure;
    }

    void SpawnStructure()
    {
        if (structures.Length == 0 || player == null) return;

        Vector2 spawnPos = GetRandomPositionAroundPlayer();
        GameObject prefab = structures[Random.Range(0, structures.Length)];

        GameObject spawned = Instantiate(prefab, spawnPos, Quaternion.identity);
        objectiveArrow.SetTarget(spawned.transform);

        worldMessage.ShowMessage("STRUCTURE DISCOVERED");
    }

    Vector2 GetRandomPositionAroundPlayer()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minDistanceFromPlayer, spawnRadius);

        return (Vector2)player.position + randomDir * distance;
    }
}
