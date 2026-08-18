using UnityEngine;

public enum ItemType
{
    Weapon,
    Passive
}

public abstract class LevelUpItemData : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private int maxLevel = 5;

    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public int MaxLevel => maxLevel;

    public abstract ItemType ItemType { get; }

    public abstract string GetLevelDescription(int level);
}