using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyDeath : MonoBehaviour
{
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnDied += Die;
    }

    private void OnDisable()
    {
        health.OnDied -= Die;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}