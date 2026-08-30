using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float moveSpeedBonus;
    private float maxHealthBonus;
    private float pickupRangeBonus;

    public float MoveSpeedMultiplier => 1f + moveSpeedBonus;
    public float MaxHealthMultiplier => 1f + maxHealthBonus;
    public float PickupRangeMultiplier => 1f + pickupRangeBonus;

    public void ResetPassiveModifiers()
    {
        moveSpeedBonus = 0f;
        maxHealthBonus = 0f;
        pickupRangeBonus = 0f;
    }

    public void AddMoveSpeedBonus(float bonus)
    {
        moveSpeedBonus += bonus;
    }

    public void AddMaxHealthBonus(float bonus)
    {
        maxHealthBonus += bonus;
    }

    public void AddPickupRangeBonus(float bonus)
    {
        pickupRangeBonus += bonus;
    }
}