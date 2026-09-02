using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerDamageReceiver : MonoBehaviour
{
    [SerializeField] private float invulnerabilityDuration = 0.5f;
    [SerializeField] private AudioSource hitAudioSource;

    private Health health;
    private float invulnerabilityTimer;

    public bool IsInvulnerable => invulnerabilityTimer > 0f;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (invulnerabilityTimer > 0f)
        {
            invulnerabilityTimer -= Time.deltaTime;
        }
    }

    public bool TryTakeDamage(float damage)
    {
        if (health.IsDead)
            return false;

        if (IsInvulnerable)
            return false;

        if (damage <= 0f)
            return false;

        health.TakeDamage(damage);

        invulnerabilityTimer = invulnerabilityDuration;

        hitAudioSource?.Play();

        Debug.Log(
            $"Player took {damage} damage. " +
            $"HP: {health.CurrentHealth}/{health.MaxHealth}"
        );

        return true;
    }
}