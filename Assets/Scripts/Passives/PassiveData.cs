using UnityEngine;

public abstract class PassiveData : LevelUpItemData
{
    public override ItemType ItemType => ItemType.Passive;

    public abstract void Apply(
        PlayerStats playerStats,
        int level
    );
}