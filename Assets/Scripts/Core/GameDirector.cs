using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Difficulty")]
    [SerializeField]
    private List<DifficultyStageData> stages = new();

    public float ElapsedTime { get; private set; }

    public int CurrentStageIndex { get; private set; } = -1;

    private void Start()
    {
        if (stages.Count == 0)
        {
            Debug.LogWarning(
                "GameDirector has no difficulty stages.",
                this
            );

            return;
        }

        SortStages();

        ApplyStage(0);
    }

    private void Update()
    {
        if (stages.Count == 0)
            return;

        ElapsedTime += Time.deltaTime;

        CheckForStageChange();
    }

    private void CheckForStageChange()
    {
        int nextStageIndex =
            CurrentStageIndex + 1;

        if (nextStageIndex >= stages.Count)
            return;

        DifficultyStageData nextStage =
            stages[nextStageIndex];

        if (ElapsedTime >= nextStage.StartTime)
        {
            ApplyStage(nextStageIndex);
        }
    }

    private void ApplyStage(int index)
    {
        if (index < 0 || index >= stages.Count)
            return;

        CurrentStageIndex = index;

        DifficultyStageData stage =
            stages[index];

        enemySpawner.SetSpawnConfiguration(
            stage.Enemies,
            stage.SpawnInterval,
            stage.MaxEnemies
        );

        enemySpawner.SetDifficultyMultipliers(
            stage.HealthMultiplier,
            stage.MoveSpeedMultiplier,
            stage.DamageMultiplier
        );

        Debug.Log(
            $"Difficulty Stage {index + 1} started " +
            $"at {ElapsedTime:0.0}s."
        );
    }

    private void SortStages()
    {
        stages.Sort(
            (a, b) =>
                a.StartTime.CompareTo(b.StartTime)
        );
    }
}