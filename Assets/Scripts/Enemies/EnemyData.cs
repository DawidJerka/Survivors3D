using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyData",
    menuName = "Survivors/Enemy"
)]
public class EnemyData : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string displayName;

    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float contactDamage = 10f;

    public string DisplayName => displayName;
    public float MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public float ContactDamage => contactDamage;
}