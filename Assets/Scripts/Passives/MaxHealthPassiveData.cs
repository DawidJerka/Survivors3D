using UnityEngine;

[CreateAssetMenu(
    fileName = "MaxHealthPassive",
    menuName = "Survivors/Passives/Max Health"
)]
public class MaxHealthPassiveData : PassiveData
{
    [Range(0f, 1f)]
    [SerializeField] private float increasePerLevel = 0.20f;

    public override void Apply(
        PlayerStats playerStats,
        int level)
    {
        float bonus = increasePerLevel * level;

        playerStats.AddMaxHealthBonus(bonus);
    }

    public override string GetLevelDescription(int level)
    {
        return $"+{increasePerLevel * 100f:0}% Max Health";
    }
}