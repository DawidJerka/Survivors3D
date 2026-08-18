using UnityEngine;

[CreateAssetMenu(
    fileName = "PassiveData",
    menuName = "Survivors/Passive"
)]
public class PassiveData : LevelUpItemData
{
    public override ItemType ItemType => ItemType.Passive;

    public override string GetLevelDescription(int level)
    {
        return $"Passive Level {level}";
    }
}