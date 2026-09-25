using UnityEngine;

public class ExperienceGem : MonoBehaviour
{
    [SerializeField] private int experienceValue = 1;

    [Header("Attraction")]
    [SerializeField] private float attractionSpeed = 8f;
    [SerializeField] private float acceleration = 20f;

    private ExperienceCollector collector;
    private float currentSpeed;

    public int ExperienceValue => experienceValue;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;

        collector = player.GetComponent<ExperienceCollector>();
    }

    private void Update()
    {
        if (collector == null)
            return;

        Vector3 toPlayer =
            collector.transform.position - transform.position;

        float distanceSqr = toPlayer.sqrMagnitude;
        float attractionRadius = collector.AttractionRadius;

        if (distanceSqr >
            attractionRadius * attractionRadius)
        {
            currentSpeed = 0f;
            return;
        }

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            attractionSpeed,
            acceleration * Time.deltaTime
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            collector.transform.position,
            currentSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        ExperienceCollector collector =
            other.GetComponent<ExperienceCollector>();

        if (collector == null)
            collector =
                other.GetComponentInParent<ExperienceCollector>();

        if (collector == null)
            return;

        collector.CollectExperience(experienceValue);

        Destroy(gameObject);
    }
}