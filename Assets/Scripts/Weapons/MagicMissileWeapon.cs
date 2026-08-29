using UnityEngine;

public class MagicMissileWeapon : TimedWeapon
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    private EnemyTargeting targeting;

    private MagicMissileData magicMissileData;

    protected override void OnInitialized()
    {
        magicMissileData = Data as MagicMissileData;

        if (magicMissileData == null)
        {
            Debug.LogError(
                "MagicMissileWeapon requires MagicMissileData.",
                this
            );

            return;
        }

        targeting = GetComponentInParent<EnemyTargeting>();
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
        return magicMissileData.GetCooldown(CurrentLevel);
    }
}