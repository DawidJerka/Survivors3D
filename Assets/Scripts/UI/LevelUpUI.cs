using System.Collections.Generic;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField] private LevelUpManager levelUpManager;

    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private Transform optionsContainer;
    [SerializeField] private LevelUpOptionUI optionPrefab;

    private readonly List<LevelUpOptionUI> spawnedOptions = new();

    private void OnEnable()
    {
        levelUpManager.OnLevelUpStarted += HandleLevelUpStarted;
        levelUpManager.OnLevelUpFinished += HandleLevelUpFinished;
    }

    private void Start()
    {
        levelUpPanel.SetActive(false);
    }

    private void OnDisable()
    {
        levelUpManager.OnLevelUpStarted -= HandleLevelUpStarted;
        levelUpManager.OnLevelUpFinished -= HandleLevelUpFinished;
    }

    private void HandleLevelUpStarted(
        IReadOnlyList<LevelUpOption> options)
    {
        Debug.Log($"LevelUpUI received {options.Count} options");

        levelUpPanel.SetActive(true);

        ClearOptions();

        for (int i = 0; i < options.Count; i++)
        {
            int optionIndex = i;

            LevelUpOptionUI optionUI = Instantiate(
                optionPrefab,
                optionsContainer
            );

            optionUI.Setup(
                options[i],
                () => levelUpManager.SelectOption(optionIndex)
            );

            spawnedOptions.Add(optionUI);
        }
    }

    private void HandleLevelUpFinished()
    {
        levelUpPanel.SetActive(false);

        ClearOptions();
    }

    private void ClearOptions()
    {
        foreach (LevelUpOptionUI option in spawnedOptions)
        {
            Destroy(option.gameObject);
        }

        spawnedOptions.Clear();
    }
}