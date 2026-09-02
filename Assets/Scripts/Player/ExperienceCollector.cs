using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerExperience))]
public class ExperienceCollector : MonoBehaviour
{
    [SerializeField] private float baseAttractionRadius = 4f;

    [Header("Audio")]
    [SerializeField] private AudioSource pickupAudioSource;

    private PlayerStats playerStats;
    private PlayerExperience playerExperience;

    public float AttractionRadius =>
        baseAttractionRadius * playerStats.PickupRangeMultiplier;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerExperience = GetComponent<PlayerExperience>();
    }

    public void CollectExperience(int amount)
    {
        if (amount <= 0)
            return;

        playerExperience.AddExperience(amount);

        pickupAudioSource?.Play();
    }
}