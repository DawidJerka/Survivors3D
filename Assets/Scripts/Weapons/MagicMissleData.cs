using UnityEngine;

[CreateAssetMenu(
    fileName = "MagicMissileData",
    menuName = "Survivors/Weapons/Magic Missile"
)]
public class MagicMissileData : WeaponData
{
    [Header("Attack Speed")]
    [SerializeField] private float baseAttackSpeed = 1f;

    [Range(0f, 1f)]
    [SerializeField] private float attackSpeedIncreasePerLevel = 0.15f;

    public float GetAttackSpeed(int level)
    {
        int upgrades = Mathf.Max(0, level - 1);

        return baseAttackSpeed *
               (1f + attackSpeedIncreasePerLevel * upgrades);
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
            return $"Attack Speed: {GetAttackSpeed(level):0.00}/s";

        return $"+{attackSpeedIncreasePerLevel * 100f:0}% Attack Speed";
    }
}