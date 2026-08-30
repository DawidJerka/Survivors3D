using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += HandleHealthChanged;
        }
    }

    private void Start()
    {
        Refresh();
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(
        float currentHealth,
        float maxHealth)
    {
        Refresh();
    }

    private void Refresh()
    {
        if (playerHealth == null)
            return;

        healthBar.maxValue = playerHealth.MaxHealth;
        healthBar.value = playerHealth.CurrentHealth;

        healthText.text =
            $"{Mathf.CeilToInt(playerHealth.CurrentHealth)} / " +
            $"{Mathf.CeilToInt(playerHealth.MaxHealth)} HP";
    }
}