using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class ExperienceCollector : MonoBehaviour
{
    [SerializeField] private float baseAttractionRadius = 4f;

    private PlayerStats playerStats;

    public float AttractionRadius =>
        baseAttractionRadius * playerStats.PickupRangeMultiplier;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }
}