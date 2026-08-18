using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperienceUI : MonoBehaviour
{
    [SerializeField] private PlayerExperience playerExperience;

    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text experienceText;
    [SerializeField] private Slider experienceBar;

    private void OnEnable()
    {
        if (playerExperience == null)
            return;

        playerExperience.OnExperienceChanged += HandleExperienceChanged;
        playerExperience.OnLevelUp += HandleLevelUp;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnDisable()
    {
        if (playerExperience == null)
            return;

        playerExperience.OnExperienceChanged -= HandleExperienceChanged;
        playerExperience.OnLevelUp -= HandleLevelUp;
    }

    private void HandleExperienceChanged(int experience)
    {
        Refresh();
    }

    private void HandleLevelUp(int level)
    {
        Refresh();
    }

    private void Refresh()
    {
        if (playerExperience == null)
            return;

        levelText.text = $"LEVEL {playerExperience.CurrentLevel}";

        experienceText.text =
            $"{playerExperience.CurrentExperience} / " +
            $"{playerExperience.ExperienceToNextLevel} XP";

        experienceBar.maxValue =
            playerExperience.ExperienceToNextLevel;

        experienceBar.value =
            playerExperience.CurrentExperience;
    }
}