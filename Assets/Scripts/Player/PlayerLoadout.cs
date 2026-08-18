using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    [SerializeField] private int maxWeapons = 6;
    [SerializeField] private int maxPassives = 6;

    private readonly Dictionary<LevelUpItemData, int> itemLevels = new();

    public int MaxWeapons => maxWeapons;
    public int MaxPassives => maxPassives;

    public int WeaponCount => GetItemCount(ItemType.Weapon);
    public int PassiveCount => GetItemCount(ItemType.Passive);

    public bool HasItem(LevelUpItemData item)
    {
        return itemLevels.ContainsKey(item);
    }

    public int GetLevel(LevelUpItemData item)
    {
        if (itemLevels.TryGetValue(item, out int level))
            return level;

        return 0;
    }

    public void AddOrUpgrade(LevelUpItemData item)
    {
        if (itemLevels.TryGetValue(item, out int level))
        {
            if (level >= item.MaxLevel)
                return;

            itemLevels[item] = level + 1;
        }
        else
        {
            itemLevels.Add(item, 1);
        }
    }

    private int GetItemCount(ItemType type)
    {
        int count = 0;

        foreach (LevelUpItemData item in itemLevels.Keys)
        {
            if (item.ItemType == type)
                count++;
        }

        return count;
    }
}