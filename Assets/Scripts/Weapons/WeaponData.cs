using UnityEngine;

[CreateAssetMenu(
    fileName = "WeaponData",
    menuName = "Survivors/Weapon"
)]
public class WeaponData : LevelUpItemData
{
    [Header("Weapon")]
    [SerializeField] private Weapon weaponPrefab;

    [Header("Attack Speed")]
    [SerializeField] private float baseAttackSpeed = 1f;

    [Range(0f, 1f)]
    [SerializeField] private float attackSpeedIncreasePerLevel = 0.15f;

    public Weapon WeaponPrefab => weaponPrefab;

    public override ItemType ItemType => ItemType.Weapon;

    public float GetAttackSpeed(int level)
    {
        int upgrades = Mathf.Max(0, level - 1);

        float attackSpeedMultiplier =
            1f + attackSpeedIncreasePerLevel * upgrades;

        return baseAttackSpeed * attackSpeedMultiplier;
    }

    public float GetCooldown(int level)
    {
        float attackSpeed = GetAttackSpeed(level);

        if (attackSpeed <= 0f)
            return Mathf.Infinity;

        return 1f / attackSpeed;
    }

    public override string GetLevelDescription(int level)
    {
        if (level <= 1)
        {
            return $"Attack Speed: {GetAttackSpeed(level):0.00}/s";
        }

        return $"+{attackSpeedIncreasePerLevel * 100f:0}% Attack Speed";
    }
}