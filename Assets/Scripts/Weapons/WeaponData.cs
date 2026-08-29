using UnityEngine;

[CreateAssetMenu(
    fileName = "WeaponData",
    menuName = "Survivors/Weapon"
)]
public class WeaponData : LevelUpItemData
{
    [SerializeField] private Weapon weaponPrefab;

    public Weapon WeaponPrefab => weaponPrefab;

    public override ItemType ItemType => ItemType.Weapon;

    public override string GetLevelDescription(int level)
    {
        return $"Weapon Level {level}";
    }
}