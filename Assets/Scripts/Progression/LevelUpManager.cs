using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerExperience playerExperience;
    [SerializeField] private PlayerLoadout playerLoadout;

    [Header("Level Up Pool")]
    [SerializeField] private List<LevelUpItemData> availableItems = new();
    [SerializeField] private int choicesCount = 3;

    public event Action<IReadOnlyList<LevelUpOption>> OnLevelUpStarted;
    public event Action OnLevelUpFinished;

    public IReadOnlyList<LevelUpOption> CurrentOptions => currentOptions;
    public bool IsLevelUpActive { get; private set; }

    private List<LevelUpOption> currentOptions = new();

    private int pendingLevelUps;

    private bool gamePaused;
    private float previousTimeScale = 1f;

    private void OnEnable()
    {
        if (playerExperience != null)
        {
            playerExperience.OnLevelUp += HandleLevelUp;
        }
    }

    private void OnDisable()
    {
        if (playerExperience != null)
        {
            playerExperience.OnLevelUp -= HandleLevelUp;
        }

        if (gamePaused)
        {
            ResumeGame();
        }
    }

    private void HandleLevelUp(int level)
    {
        pendingLevelUps++;

        if (!IsLevelUpActive)
        {
            StartNextLevelUp();
        }
    }

    private void StartNextLevelUp()
    {
        if (pendingLevelUps <= 0)
        {
            FinishLevelUps();
            return;
        }

        currentOptions = GetRandomOptions(choicesCount);

        // Jeżeli wszystko jest już na max levelu,
        // nie ma czego zaoferować.
        if (currentOptions.Count == 0)
        {
            pendingLevelUps--;
            StartNextLevelUp();
            return;
        }

        IsLevelUpActive = true;

        PauseGame();

        OnLevelUpStarted?.Invoke(currentOptions);
    }

    public void SelectOption(int index)
    {
        if (!IsLevelUpActive)
            return;

        if (index < 0 || index >= currentOptions.Count)
            return;

        LevelUpOption selectedOption = currentOptions[index];

        playerLoadout.AddOrUpgrade(selectedOption.Item);

        pendingLevelUps--;

        IsLevelUpActive = false;
        currentOptions.Clear();

        if (pendingLevelUps > 0)
        {
            StartNextLevelUp();
        }
        else
        {
            FinishLevelUps();
        }
    }

    private List<LevelUpOption> GetEligibleOptions()
    {
        List<LevelUpOption> options = new();
        HashSet<LevelUpItemData> processedItems = new();

        foreach (LevelUpItemData item in availableItems)
        {
            if (item == null)
                continue;

            // Chroni przed przypadkowym dodaniem tego samego
            // ScriptableObjectu kilka razy do listy.
            if (!processedItems.Add(item))
                continue;

            int currentLevel = playerLoadout.GetLevel(item);

            // Gracz już posiada przedmiot.
            if (currentLevel > 0)
            {
                if (currentLevel < item.MaxLevel)
                {
                    options.Add(
                        new LevelUpOption(item, currentLevel)
                    );
                }

                continue;
            }

            // Gracz jeszcze nie posiada przedmiotu.
            switch (item.ItemType)
            {
                case ItemType.Weapon:
                    if (playerLoadout.WeaponCount < playerLoadout.MaxWeapons)
                    {
                        options.Add(
                            new LevelUpOption(item, 0)
                        );
                    }

                    break;

                case ItemType.Passive:
                    if (playerLoadout.PassiveCount < playerLoadout.MaxPassives)
                    {
                        options.Add(
                            new LevelUpOption(item, 0)
                        );
                    }

                    break;
            }
        }

        return options;
    }

    private List<LevelUpOption> GetRandomOptions(int count)
    {
        List<LevelUpOption> options = GetEligibleOptions();

        // Partial Fisher-Yates shuffle.
        // Nie musimy tasować całej listy, jeśli potrzebujemy tylko 3 elementów.
        int numberOfChoices = Mathf.Min(count, options.Count);

        for (int i = 0; i < numberOfChoices; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, options.Count);

            (options[i], options[randomIndex]) =
                (options[randomIndex], options[i]);
        }

        return options.GetRange(0, numberOfChoices);
    }

    private void PauseGame()
    {
        if (gamePaused)
            return;

        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        gamePaused = true;
    }

    private void ResumeGame()
    {
        if (!gamePaused)
            return;

        Time.timeScale = previousTimeScale;

        gamePaused = false;
    }

    private void FinishLevelUps()
    {
        IsLevelUpActive = false;
        currentOptions.Clear();

        ResumeGame();

        OnLevelUpFinished?.Invoke();
    }
}