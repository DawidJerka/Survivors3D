using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private GameObject impactSfxPrefab;
    [SerializeField] private GameObject impactVfxPrefab;

    private Rigidbody rb;
    private bool hasHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 direction)
    {
        hasHit = false;

        rb.linearVelocity =
            direction.normalized * speed;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;

        if (!other.CompareTag("Enemy"))
            return;

        Health health =
            other.GetComponentInParent<Health>();

        if (health == null)
            return;

        hasHit = true;

        health.TakeDamage(damage);

        Vector3 hitPosition = other.ClosestPoint(transform.position);

        if (impactVfxPrefab != null)
        {
            Instantiate(
                impactVfxPrefab,
                hitPosition,
                Quaternion.identity
            );
        }

        if (impactSfxPrefab != null)
        {
            Instantiate(
                impactSfxPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        Vector3 hitDirection = rb.linearVelocity.normalized;


        EnemyHitFeedback hitFeedback =
            other.GetComponentInParent<EnemyHitFeedback>();

        if (hitFeedback != null)
        {
            hitFeedback.PlayHit(hitDirection);
        }

        Destroy(gameObject);
    }
}