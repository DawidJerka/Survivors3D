using UnityEngine;

[CreateAssetMenu(
    fileName = "MovementSpeedPassive",
    menuName = "Survivors/Passives/Movement Speed"
)]
public class MovementSpeedPassiveData : PassiveData
{
    [Range(0f, 1f)]
    [SerializeField] private float increasePerLevel = 0.10f;

    public override void Apply(
        PlayerStats playerStats,
        int level)
    {
        float bonus = increasePerLevel * level;

        playerStats.AddMoveSpeedBonus(bonus);
    }

    public override string GetLevelDescription(int level)
    {
        return $"+{increasePerLevel * 100f:0}% Movement Speed";
    }
}