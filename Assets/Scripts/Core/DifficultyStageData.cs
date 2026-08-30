using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "DifficultyStage",
    menuName = "Survivors/Difficulty Stage"
)]
public class DifficultyStageData : ScriptableObject
{
    [Header("Timing")]
    [Min(0f)]
    [SerializeField] private float startTime;

    [Header("Spawning")]
    [Min(0.05f)]
    [SerializeField] private float spawnInterval = 1f;

    [Min(1)]
    [SerializeField] private int maxEnemies = 50;

    [SerializeField]
    private List<EnemySpawnEntry> enemies = new();

    [Header("Enemy Multipliers")]
    [Min(0.01f)]
    [SerializeField] private float healthMultiplier = 1f;

    [Min(0.01f)]
    [SerializeField] private float moveSpeedMultiplier = 1f;

    [Min(0f)]
    [SerializeField] private float damageMultiplier = 1f;

    public float StartTime => startTime;
    public float SpawnInterval => spawnInterval;
    public int MaxEnemies => maxEnemies;

    public IReadOnlyList<EnemySpawnEntry> Enemies => enemies;

    public float HealthMultiplier => healthMultiplier;
    public float MoveSpeedMultiplier => moveSpeedMultiplier;
    public float DamageMultiplier => damageMultiplier;
}