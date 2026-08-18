using UnityEngine;

[CreateAssetMenu(
    fileName = "WeaponData",
    menuName = "Survivors/Weapon"
)]
public class WeaponData : LevelUpItemData
{
    public override ItemType ItemType => ItemType.Weapon;

    public override string GetLevelDescription(int level)
    {
        return $"Weapon Level {level}";
    }
}