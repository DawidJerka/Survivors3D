public class LevelUpOption
{
    public LevelUpItemData Item { get; }
    public int CurrentLevel { get; }

    public int TargetLevel => CurrentLevel + 1;

    public bool IsNew => CurrentLevel == 0;

    public LevelUpOption(
        LevelUpItemData item,
        int currentLevel)
    {
        Item = item;
        CurrentLevel = currentLevel;
    }
}