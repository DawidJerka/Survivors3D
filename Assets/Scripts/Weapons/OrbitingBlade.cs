using System.Collections.Generic;
using UnityEngine;

public class OrbitingBlade : MonoBehaviour
{
    [SerializeField] private AudioSource hitAudioSource;
    private float damage;

    private readonly HashSet<Health> hitEnemies = new();
    [SerializeField] private GameObject impactVfxPrefab;

    public void Initialize(float damage)
    {
        this.damage = damage;
        hitEnemies.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        Health health = other.GetComponentInParent<Health>();

        if (health == null || health.IsDead)
            return;

        // Jedno ostrze nie powinno zadawać obrażeń
        // temu samemu przeciwnikowi co klatkę.
        if (!hitEnemies.Add(health))
            return;

        Vector3 hitPosition =
            other.ClosestPoint(transform.position);

        Vector3 hitDirection =
            (other.transform.position - transform.position).normalized;

        EnemyHitFeedback hitFeedback =
            other.GetComponentInParent<EnemyHitFeedback>();

        health.TakeDamage(damage);

        if (hitFeedback != null)
        {
            hitFeedback.PlayHit(hitDirection);
        }

        if (impactVfxPrefab != null)
        {
            Instantiate(
                impactVfxPrefab,
                hitPosition,
                Quaternion.identity
            );
        }

        hitAudioSource?.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();

        if (health != null)
        {
            hitEnemies.Remove(health);
        }
    }

    private void OnDisable()
    {
        hitEnemies.Clear();
    }
}