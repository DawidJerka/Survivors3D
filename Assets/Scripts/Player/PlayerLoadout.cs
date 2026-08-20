using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    [Header("Limits")]
    [SerializeField] private int maxWeapons = 6;
    [SerializeField] private int maxPassives = 6;

    [Header("Starting Items")]
    [SerializeField]
    private List<LevelUpItemData> startingItems = new();

    private readonly Dictionary<LevelUpItemData, int>
        itemLevels = new();

    public event Action<LevelUpItemData, int>
        OnItemLevelChanged;

    public int MaxWeapons => maxWeapons;
    public int MaxPassives => maxPassives;

    public int WeaponCount =>
        GetItemCount(ItemType.Weapon);

    public int PassiveCount =>
        GetItemCount(ItemType.Passive);

    public IEnumerable<LevelUpItemData> OwnedItems =>
        itemLevels.Keys;

    private void Awake()
    {
        InitializeStartingItems();
    }

    private void InitializeStartingItems()
    {
        foreach (LevelUpItemData item in startingItems)
        {
            if (item == null)
                continue;

            if (itemLevels.ContainsKey(item))
                continue;

            itemLevels.Add(item, 1);
        }
    }

    public bool HasItem(LevelUpItemData item)
    {
        return itemLevels.ContainsKey(item);
    }

    public int GetLevel(LevelUpItemData item)
    {
        if (itemLevels.TryGetValue(
                item,
                out int level))
        {
            return level;
        }

        return 0;
    }

    public void AddOrUpgrade(LevelUpItemData item)
    {
        if (item == null)
            return;

        if (itemLevels.TryGetValue(
                item,
                out int currentLevel))
        {
            if (currentLevel >= item.MaxLevel)
                return;

            int newLevel = currentLevel + 1;

            itemLevels[item] = newLevel;

            OnItemLevelChanged?.Invoke(
                item,
                newLevel
            );
        }
        else
        {
            itemLevels.Add(item, 1);

            OnItemLevelChanged?.Invoke(
                item,
                1
            );
        }
    }

    private int GetItemCount(ItemType type)
    {
        int count = 0;

        foreach (LevelUpItemData item in itemLevels.Keys)
        {
            if (item.ItemType == type)
            {
                count++;
            }
        }

        return count;
    }
}