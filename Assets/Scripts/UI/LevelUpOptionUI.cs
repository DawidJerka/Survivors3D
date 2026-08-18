using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpOptionUI : MonoBehaviour
{
    [SerializeField] private Button button;

    [Header("Content")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text descriptionText;

    private Action onSelected;

    private void Awake()
    {
        button.onClick.AddListener(HandleClicked);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(HandleClicked);
    }

    public void Setup(
        LevelUpOption option,
        Action onSelected)
    {
        this.onSelected = onSelected;

        nameText.text = option.Item.DisplayName;

        if (option.IsNew)
        {
            statusText.text = "NEW";
        }
        else
        {
            statusText.text =
                $"LEVEL {option.CurrentLevel} → {option.TargetLevel}";
        }

        descriptionText.text =
            option.Item.GetLevelDescription(option.TargetLevel);

        if (option.Item.Icon != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = option.Item.Icon;
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }

    private void HandleClicked()
    {
        onSelected?.Invoke();
    }
}