using UnityEngine;

public abstract class TimedWeapon : Weapon
{
    private float attackTimer;

    protected virtual void Update()
    {
        if (!IsInitialized)
            return;

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            return;
        }

        if (Attack())
        {
            attackTimer = GetCooldown();
        }
    }

    protected abstract bool Attack();

    protected abstract float GetCooldown();
}