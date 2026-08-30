using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float moveSpeedBonus;
    private float maxHealthBonus;

    public float MoveSpeedMultiplier => 1f + moveSpeedBonus;
    public float MaxHealthMultiplier => 1f + maxHealthBonus;

    public void ResetPassiveModifiers()
    {
        moveSpeedBonus = 0f;
        maxHealthBonus = 0f;
    }

    public void AddMoveSpeedBonus(float bonus)
    {
        moveSpeedBonus += bonus;
    }

    public void AddMaxHealthBonus(float bonus)
    {
        maxHealthBonus += bonus;
    }
}