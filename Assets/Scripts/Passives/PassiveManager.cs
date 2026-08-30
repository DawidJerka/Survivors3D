using UnityEngine;

[RequireComponent(typeof(PlayerLoadout))]
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(Health))]
public class PassiveManager : MonoBehaviour
{
    private PlayerLoadout playerLoadout;
    private PlayerStats playerStats;
    private Health playerHealth;

    private void Awake()
    {
        playerLoadout = GetComponent<PlayerLoadout>();
        playerStats = GetComponent<PlayerStats>();
        playerHealth = GetComponent<Health>();
    }

    private void OnEnable()
    {
        playerLoadout.OnItemLevelChanged +=
            HandleItemLevelChanged;
    }

    private void Start()
    {
        RecalculatePassives();
    }

    private void OnDisable()
    {
        playerLoadout.OnItemLevelChanged -=
            HandleItemLevelChanged;
    }

    private void HandleItemLevelChanged(
        LevelUpItemData item,
        int newLevel)
    {
        if (item is not PassiveData)
            return;

        RecalculatePassives();
    }

    private void RecalculatePassives()
    {
        playerStats.ResetPassiveModifiers();

        foreach (LevelUpItemData item
                 in playerLoadout.OwnedItems)
        {
            if (item is not PassiveData passive)
                continue;

            int level =
                playerLoadout.GetLevel(passive);

            passive.Apply(
                playerStats,
                level
            );
        }

        playerHealth.SetMaxHealthMultiplier(
            playerStats.MaxHealthMultiplier
        );
    }
}