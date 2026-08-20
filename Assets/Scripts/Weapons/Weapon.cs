using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData Data { get; private set; }

    protected PlayerLoadout PlayerLoadout { get; private set; }

    protected bool IsInitialized { get; private set; }

    protected int CurrentLevel =>
        PlayerLoadout != null && Data != null
            ? PlayerLoadout.GetLevel(Data)
            : 0;

    public virtual void Initialize(
        WeaponData data,
        PlayerLoadout playerLoadout)
    {
        Data = data;
        PlayerLoadout = playerLoadout;

        IsInitialized = true;

        OnInitialized();
    }

    protected virtual void OnInitialized()
    {
    }

    public virtual void HandleLevelChanged(int newLevel)
    {
    }
}