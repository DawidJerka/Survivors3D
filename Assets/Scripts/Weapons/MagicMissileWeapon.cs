using UnityEngine;

public class MagicMissileWeapon : TimedWeapon
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    private EnemyTargeting targeting;

    protected override void OnInitialized()
    {
        targeting = GetComponentInParent<EnemyTargeting>();

        if (targeting == null)
        {
            Debug.LogError(
                $"{nameof(MagicMissileWeapon)} requires " +
                $"{nameof(EnemyTargeting)} on the Player.",
                this
            );
        }
    }

    protected override bool Attack()
    {
        if (targeting == null || projectilePrefab == null)
            return false;

        Transform target = targeting.GetNearestEnemy();

        if (target == null)
            return false;

        Vector3 spawnPosition =
            projectileSpawnPoint != null
                ? projectileSpawnPoint.position
                : transform.position;

        Vector3 direction =
            target.position - spawnPosition;

        direction.y = 0f;

        Projectile projectile = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.identity
        );

        projectile.Initialize(direction);

        return true;
    }

    protected override float GetCooldown()
    {
        return Data.GetCooldown(CurrentLevel);
    }
}