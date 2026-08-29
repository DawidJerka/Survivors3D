using UnityEngine;

[CreateAssetMenu(
    fileName = "OrbitingBladesData",
    menuName = "Survivors/Weapons/Orbiting Blades"
)]
public class OrbitingBladesData : WeaponData
{
    [Header("Blades")]
    [SerializeField] private int baseBladeCount = 2;
    [SerializeField] private int bladesAddedPerLevel = 1;

    [Header("Orbit")]
    [SerializeField] private float orbitRadius = 2.5f;
    [SerializeField] private float orbitHeight = 0.75f;
    [SerializeField] private float rotationSpeed = 120f;

    [Header("Damage")]
    [SerializeField] private float damage = 20f;

    public float OrbitRadius => orbitRadius;
    public float OrbitHeight => orbitHeight;
    public float RotationSpeed => rotationSpeed;
    public float Damage => damage;

    public int GetBladeCount(int level)
    {
        int upgrades = Mathf.Max(0, level - 1);

        return baseBladeCount +
               bladesAddedPerLevel * upgrades;
    }

    public override string GetLevelDescription(int level)
    {
        if (level <= 1)
            return $"{GetBladeCount(level)} orbiting blades";

        return $"+{bladesAddedPerLevel} Blade";
    }
}