using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Spawning")]
    [SerializeField] private float spawnDistance = 20f;

    private float spawnInterval = 1f;
    private int maxEnemies = 50;

    private float healthMultiplier = 1f;
    private float moveSpeedMultiplier = 1f;
    private float damageMultiplier = 1f;

    private IReadOnlyList<EnemySpawnEntry> enemies;

    private float spawnTimer;
    private int aliveEnemies;

    private void Update()
    {
        if (enemies == null || enemies.Count == 0)
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    public void SetSpawnConfiguration(
        IReadOnlyList<EnemySpawnEntry> spawnEntries,
        float interval,
        int maximumEnemies)
    {
        enemies = spawnEntries;

        spawnInterval = Mathf.Max(
            0.05f,
            interval
        );

        maxEnemies = Mathf.Max(
            1,
            maximumEnemies
        );
    }

    public void SetDifficultyMultipliers(
        float health,
        float moveSpeed,
        float damage)
    {
        healthMultiplier = Mathf.Max(0.01f, health);
        moveSpeedMultiplier = Mathf.Max(0.01f, moveSpeed);
        damageMultiplier = Mathf.Max(0f, damage);
    }

    private void SpawnEnemy()
    {
        if (aliveEnemies >= maxEnemies)
            return;

        GameObject enemyPrefab =
            GetRandomEnemyPrefab();

        if (enemyPrefab == null)
            return;

        Vector2 randomDirection =
            Random.insideUnitCircle.normalized;

        Vector3 spawnPosition =
            player.position +
            new Vector3(
                randomDirection.x,
                0f,
                randomDirection.y
            ) * spawnDistance;

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        EnemyInitializer initializer =
            enemy.GetComponent<EnemyInitializer>();

        if (initializer != null)
        {
            initializer.SetDifficultyMultipliers(
                healthMultiplier,
                moveSpeedMultiplier,
                damageMultiplier
            );
        }

        aliveEnemies++;

        Health health =
            enemy.GetComponent<Health>();

        if (health != null)
        {
            health.OnDied += HandleEnemyDied;
        }
    }

    private GameObject GetRandomEnemyPrefab()
    {
        float totalWeight = 0f;

        foreach (EnemySpawnEntry entry in enemies)
        {
            if (entry.Prefab != null)
            {
                totalWeight += entry.Weight;
            }
        }

        if (totalWeight <= 0f)
            return null;

        float randomValue =
            Random.Range(0f, totalWeight);

        float currentWeight = 0f;

        foreach (EnemySpawnEntry entry in enemies)
        {
            if (entry.Prefab == null)
                continue;

            currentWeight += entry.Weight;

            if (randomValue <= currentWeight)
            {
                return entry.Prefab;
            }
        }

        return null;
    }

    private void HandleEnemyDied()
    {
        aliveEnemies = Mathf.Max(
            0,
            aliveEnemies - 1
        );
    }
}