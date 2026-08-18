using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private ExperienceGem experienceGemPrefab;
    [SerializeField] private Transform experienceDropPoint;

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
        DropExperience();

        Destroy(gameObject);
    }

    private void DropExperience()
    {
        if (experienceGemPrefab == null)
            return;

        Vector3 dropPosition = experienceDropPoint != null
            ? experienceDropPoint.position
            : transform.position;

        Instantiate(
            experienceGemPrefab,
            dropPosition,
            Quaternion.identity
        );
    }
}