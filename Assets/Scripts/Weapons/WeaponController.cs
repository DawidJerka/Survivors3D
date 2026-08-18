using UnityEngine;

[RequireComponent(typeof(EnemyTargeting))]
public class WeaponController : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float attackCooldown = 1f;

    private EnemyTargeting targeting;
    private float attackTimer;

    private void Awake()
    {
        targeting = GetComponent<EnemyTargeting>();
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    private void Attack()
    {
        Transform target = targeting.GetNearestEnemy();

        if (target == null)
            return;

        Vector3 direction = target.position - projectileSpawnPoint.position;
        direction.y = 0f;

        Projectile projectile = Instantiate(
            projectilePrefab,
            projectileSpawnPoint.position,
            Quaternion.identity
        );

        projectile.Initialize(direction);
    }
}