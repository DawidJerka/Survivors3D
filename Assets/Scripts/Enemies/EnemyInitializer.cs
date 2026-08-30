using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyContactDamage))]
public class EnemyInitializer : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    private Health health;
    private EnemyMovement movement;
    private EnemyContactDamage contactDamage;

    private float healthMultiplier = 1f;
    private float moveSpeedMultiplier = 1f;
    private float damageMultiplier = 1f;

    private void Awake()
    {
        health = GetComponent<Health>();
        movement = GetComponent<EnemyMovement>();
        contactDamage = GetComponent<EnemyContactDamage>();
    }

    private void Start()
    {
        if (enemyData == null)
        {
            Debug.LogError(
                $"{name} has no EnemyData assigned.",
                this
            );

            return;
        }

        ApplyStats();
    }

    public void SetDifficultyMultipliers(
        float health,
        float moveSpeed,
        float damage)
    {
        healthMultiplier = Mathf.Max(0.01f, health);
        moveSpeedMultiplier = Mathf.Max(0.01f, moveSpeed);
        damageMultiplier = Mathf.Max(0f, damage);
    }

    private void ApplyStats()
    {
        health.SetBaseMaxHealth(
            enemyData.MaxHealth * healthMultiplier
        );

        movement.SetMoveSpeed(
            enemyData.MoveSpeed * moveSpeedMultiplier
        );

        contactDamage.SetDamage(
            enemyData.ContactDamage * damageMultiplier
        );
    }
}