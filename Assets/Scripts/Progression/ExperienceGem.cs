using UnityEngine;

public class ExperienceGem : MonoBehaviour
{
    [SerializeField] private int experienceValue = 1;

    public int ExperienceValue => experienceValue;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerExperience playerExperience =
            other.GetComponent<PlayerExperience>();

        if (playerExperience == null)
            return;

        playerExperience.AddExperience(experienceValue);

        Destroy(gameObject);
    }
}