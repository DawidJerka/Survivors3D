using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float damage = 25f;

    private Rigidbody rb;
    private bool hasHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 direction)
    {
        rb.linearVelocity = direction.normalized * speed;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;

        if (!other.CompareTag("Enemy"))
            return;

        Health health = other.GetComponent<Health>();

        if (health == null)
            return;

        hasHit = true;

        health.TakeDamage(damage);

        Destroy(gameObject);
    }
}