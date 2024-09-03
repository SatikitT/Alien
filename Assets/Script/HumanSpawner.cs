using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    public GameObject[] humanPrefabs; // Array of different human prefabs
    public float spawnInterval = 2f; // Time interval between spawns
    public Vector2 spawnAreaMin; // Minimum corner of the spawn area
    public Vector2 spawnAreaMax; // Maximum corner of the spawn area

    private float timeSinceLastSpawn;

    void Start()
    {
        timeSinceLastSpawn = 0f;
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnHuman();
            timeSinceLastSpawn = 0f;
        }
    }

    void SpawnHuman()
    {
        // Choose a random human prefab
        int randomIndex = Random.Range(0, humanPrefabs.Length);
        GameObject humanPrefab = humanPrefabs[randomIndex];

        // Choose a random position within the spawn area
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        // Instantiate the human prefab at the chosen position
        Instantiate(humanPrefab, spawnPosition, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector3)spawnAreaMin + (Vector3)spawnAreaMax / 2, new Vector3(spawnAreaMax.x - spawnAreaMin.x, spawnAreaMax.y - spawnAreaMin.y, 1));
    }
}
