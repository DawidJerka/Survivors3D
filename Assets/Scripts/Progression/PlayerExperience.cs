using System;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField] private int startingExperienceToNextLevel = 5;
    [SerializeField] private float experienceRequirementMultiplier = 1.5f;

    public int CurrentLevel { get; private set; } = 1;
    public int CurrentExperience { get; private set; }
    public int ExperienceToNextLevel { get; private set; }

    public event Action<int> OnExperienceChanged;
    public event Action<int> OnLevelUp;

    private void Awake()
    {
        ExperienceToNextLevel = startingExperienceToNextLevel;
    }

    public void AddExperience(int amount)
    {
        if (amount <= 0)
            return;

        CurrentExperience += amount;

        while (CurrentExperience >= ExperienceToNextLevel)
        {
            LevelUp();
        }

        OnExperienceChanged?.Invoke(CurrentExperience);
    }

    private void LevelUp()
    {
        CurrentExperience -= ExperienceToNextLevel;

        CurrentLevel++;

        ExperienceToNextLevel = Mathf.CeilToInt(
            ExperienceToNextLevel * experienceRequirementMultiplier
        );

        OnLevelUp?.Invoke(CurrentLevel);

        Debug.Log($"Level up! Current level: {CurrentLevel}");
    }
}