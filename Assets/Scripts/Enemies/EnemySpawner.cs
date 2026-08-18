using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;

    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnDistance = 20f;
    [SerializeField] private int maxEnemies = 50;

    private float spawnTimer;
    private int aliveEnemies;

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        if (aliveEnemies >= maxEnemies)
            return;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        Vector3 spawnPosition = player.position + new Vector3(
            randomDirection.x,
            0f,
            randomDirection.y
        ) * spawnDistance;

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        aliveEnemies++;

        Health health = enemy.GetComponent<Health>();

        if (health != null)
        {
            health.OnDied += HandleEnemyDied;
        }
    }

    private void HandleEnemyDied()
    {
        aliveEnemies--;
    }
}